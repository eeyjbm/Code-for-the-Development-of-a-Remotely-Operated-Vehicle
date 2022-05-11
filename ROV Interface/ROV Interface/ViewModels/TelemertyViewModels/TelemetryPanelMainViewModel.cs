using ROV_Interface.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.ViewModels.TelemertyViewModels
{
    public class TelemetryPanelMainViewModel : ViewModelBase
    {

        private readonly RemoteStateStore _remoteStateStore;
        private readonly TelemetryStateStore _telemetryStateStore;

        private int depth;
        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
                OnPropertyChanged("Depth");
            }
        }

        private double temperature;
        public double Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        private int servoPan;
        public int ServoPan
        {
            get
            {
                return servoPan;
            }
            set
            {
                servoPan = value;
                OnPropertyChanged("ServoPan");
            }
        }

        private int servoTilt;
        public int ServoTilt
        {
            get
            {
                return servoTilt;
            }
            set
            {
                servoTilt = value;
                OnPropertyChanged("ServoTilt");
            }
        }

        private int pitch;
        public int Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                OnPropertyChanged("Pitch");
            }
        }

        private int roll;
        public int Roll
        {
            get
            {
                return roll;
            }
            set
            {
                roll = value;
                OnPropertyChanged("Roll");
            }
        }

        private double yaw;
        public double Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = value;
                OnPropertyChanged("Yaw");
            }
        }

        private double cell1;
        public double Cell1
        {
            get
            {
                return cell1;
            }
            set
            {
                cell1 = value;
                OnPropertyChanged("Cell1");
            }
        }

        private double cell2;
        public double Cell2
        {
            get
            {
                return cell2;
            }
            set
            {
                cell2 = value;
                OnPropertyChanged("Cell2");
            }
        }

        private double cell3;
        public double Cell3
        {
            get
            {
                return cell3;
            }
            set
            {
                cell3 = value;
                OnPropertyChanged("Cell3");
            }
        }

        private double cell4;
        public double Cell4
        {
            get
            {
                return cell4;
            }
            set
            {
                cell4 = value;
                OnPropertyChanged("Cell4");
            }
        }

        private double cell5;
        public double Cell5
        {
            get
            {
                return cell5;
            }
            set
            {
                cell5 = value;
                OnPropertyChanged("Cell5");
            }
        }

        private double battery;
        public double Battery
        {
            get
            {
                return battery;
            }
            set
            {
                battery = value;
                OnPropertyChanged("Battery");
            }
        }


        public TelemetryPanelMainViewModel(RemoteStateStore remoteStateStore, TelemetryStateStore telemetryStateStore)
        {
            this._remoteStateStore = remoteStateStore;
            this._telemetryStateStore = telemetryStateStore;
            _remoteStateStore.SendRemoteDataEvent += _remoteStateStore_SendRemoteDataEvent;
            _telemetryStateStore.SendTelemetryDataEvent += _telemetryStateStore_SendTelemetryDataEvent;
            _telemetryStateStore.SendTelemetryAndBatteryDataEvent += _telemetryStateStore_SendTelemetryAndBatteryDataEvent;
            ServoPan = 90;
            ServoTilt = 90;
            Depth = 0;
            Temperature = 0;
            Pitch = 0;
            Roll = 0;
            Yaw = 0;
            Cell1 = 0;
            Cell2 = 0;
            Cell3 = 0;
            Cell4 = 0;
            Cell5 = 0;
            Battery = 0;
        }

        private void _telemetryStateStore_SendTelemetryAndBatteryDataEvent(Models.TelemetryDataModel obj)
        {
            Depth = obj.Depth;
            Temperature = obj.Temperature;
            Pitch = obj.Pitch;
            Roll = obj.Roll;
            Yaw = obj.Yaw;

            Cell1 = obj.Cell1;
            Cell2 = obj.Cell2;
            Cell3 = obj.Cell3;
            Cell4 = obj.Cell4;
            Cell5 = obj.Cell5;

            Battery = Cell1 + Cell2 + Cell3 + Cell4 + Cell5;


        }

        private void _telemetryStateStore_SendTelemetryDataEvent(Models.TelemetryDataModel obj)
        {
            Depth = obj.Depth;
            Temperature = obj.Temperature;
            Pitch = obj.Pitch;
            Roll = obj.Roll;
            Yaw = obj.Yaw;
        }

        private void _remoteStateStore_SendRemoteDataEvent(Models.RemoteData obj)
        {
            if (obj.ARM)
            {
                ServoPan = 180 - (obj.ServoP * 5 + 35);
                ServoTilt = obj.ServoT * 10;
            }
            else
            {
                ServoPan = 90;
                ServoTilt = 90;
            }

        }
    }
}
