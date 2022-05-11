using ROV_Interface.Stores;
using ROV_Interface.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace ROV_Interface.Services 
{
    public class TCPClientService : IDisposable
    {
        private readonly RemoteStateStore _remoteStateStore;
        private readonly TelemetryStateStore _telemetryStateStore;
        private readonly SettingsStore _settingsStore;
        private readonly TCPClientStore _tCPClientStore;


        TelemetryDataModel TelemetryData;
        Socket client;
        byte[] ReceivedTelemetry = new byte[30];
        bool ClientSetup = false;
        bool typeOfData;
        public TCPClientService(RemoteStateStore remoteStateStore, TelemetryStateStore telemetryStateStore, SettingsStore settingsStore , TCPClientStore tCPClientStore)
        {
            this._remoteStateStore = remoteStateStore;
            this._telemetryStateStore = telemetryStateStore;
            this._settingsStore = settingsStore;
            this._tCPClientStore = tCPClientStore;

            _remoteStateStore.SendRemoteDataEvent += _remoteStateStore_SendRemoteDataEvent;
            _settingsStore.SendSettingsDataEvent += _settingsStore_SendSettingsDataEvent;

            TelemetryData = new TelemetryDataModel();
        }

        private void _settingsStore_SendSettingsDataEvent(SettingsDataModel obj)
        {
            if (ClientSetup)
            {
                if (client.Connected)
                {
                    try
                    {
                        client.Send(Encoding.ASCII.GetBytes("T" + obj.RecoveryTimeDelay + "P" + obj.RecoveryThrustorPower + "W" + obj.WaterDensity + "R" + Convert.ToInt16(obj.RecoveryON)  ));
                        client.Send(Encoding.ASCII.GetBytes("P" + (int)obj.PTerm + "D" + (int)obj.DTerm + "I" + (int)obj.ITerm + "T" + obj.MaxThrusterChange));

                    }
                    catch (Exception e)
                    { }
                }
            }
        }

        public bool ConnectToPI()
        {
            /*IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAdress = IPAddress.Parse(Properties.Settings.Default.ServerIP);
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, Properties.Settings.Default.ServerPort);

            client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(ipEndpoint);
                ClientSetup = true;
            }
            catch (Exception e)
            {
                if (!client.Connected)
                    return false;
            }
            return true;*/
            if (!ClientSetup)
            {


                IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAdress = IPAddress.Parse(Properties.Settings.Default.ServerIP);
                IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, Properties.Settings.Default.ServerPort);

                client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


                IAsyncResult result = client.BeginConnect(ipEndpoint, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                if (success)
                {
                    try
                    {
                        client.EndConnect(result);
                        ClientSetup = true;
                    }
                    catch (Exception e)
                    {
                        ClientSetup = false;
                        client.Close();
                    }
                }
            }
            return ClientSetup;
         

        }
        private void _remoteStateStore_SendRemoteDataEvent (Models.RemoteData obj)
        {
           Debug.WriteLine("H" + (int)obj.Heave + "S" + (int)obj.Surge + "Y" + (int)obj.Yaw + "T" + obj.ServoT + "P" + obj.ServoP + "L" + obj.Lights + Convert.ToInt16(obj.ARM) + Convert.ToInt16(obj.PositionHolding));

            if (ClientSetup)
            {
                if (client.Connected)
                {
                    try
                    {

                        client.Send(Encoding.ASCII.GetBytes("H" + (int)obj.Heave + "S" + (int)obj.Surge + "Y" + (int)obj.Yaw + "T" + obj.ServoT + "P" + obj.ServoP + "L" + obj.Lights + Convert.ToInt16(obj.ARM) + Convert.ToInt16(obj.PositionHolding)));

                        client.Receive(ReceivedTelemetry);

                        typeOfData = TelemetryData.ProcessTelemetryData(Encoding.ASCII.GetString(ReceivedTelemetry));
                        if (!typeOfData)
                        {
                            client.Receive(ReceivedTelemetry);
                            _telemetryStateStore.SendTelemetryAndBatteryData(TelemetryData);
                        }
                        else
                        {
                            _telemetryStateStore.SendTelemetryData(TelemetryData);
                        }
                       // Debug.WriteLine(Encoding.ASCII.GetString(ReceivedTelemetry));
                       // Debug.WriteLine("");
                        //TelemetryDataModelInstance.DecodeTelemetryData(Encoding.ASCII.GetString(ReceivedTelemetry));
                    }
                    catch (Exception e)
                    {
                    }
                }
                else
                {
                    if (ClientSetup)
                    {
                        _tCPClientStore.SendROVConnected(false);
                        ClientSetup = false;
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                }
            }

        }

        public void DisconnectFromPI()
        {
            if (ClientSetup)
            {
                ClientSetup = false;
                client.Send(Encoding.ASCII.GetBytes("Z"));
                Thread.Sleep(1000);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            _remoteStateStore.SendRemoteDataEvent -= _remoteStateStore_SendRemoteDataEvent;
        }

        public void Dispose()
        {
            if (ClientSetup)
            {
                ClientSetup = false;
                client.Send(Encoding.ASCII.GetBytes("Z"));
                Thread.Sleep(1000);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            _remoteStateStore.SendRemoteDataEvent -= _remoteStateStore_SendRemoteDataEvent;
        }




    }
}
