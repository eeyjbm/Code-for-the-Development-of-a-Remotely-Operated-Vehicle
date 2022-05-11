using Windows.Gaming.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROV_Interface.Stores;
using System.Diagnostics;
using System.Threading;
using ROV_Interface.Models;
using System.Timers;
using System.Collections;

namespace ROV_Interface.Services
{
    public class RemoteService
    {
        private const int TASK_ITERATION_DELAY_MS = 20;
        private CancellationTokenSource _cts;
        Gamepad controller;
        bool gamepadConnected = false;
        RemoteData _remoteData = new RemoteData();
        RemoteStateStore _remoteStateStore;

        public RemoteService(RemoteStateStore remoteStateStore)
        {
            this._remoteStateStore = remoteStateStore;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;


            System.Timers.Timer newTimer = new System.Timers.Timer();
            newTimer.Elapsed += new ElapsedEventHandler(SendRemoteData);
            newTimer.Interval = 100;
            newTimer.Start();
        }

        public void ConnectToRemote()
        {
            if (Gamepad.Gamepads.Count() > 0 && gamepadConnected == false)
            {
                gamepadConnected = true;
                controller = Gamepad.Gamepads.First();
                StartPolling();
                _remoteStateStore.RemoteConnected(true);

            }

            if (Gamepad.Gamepads.Count() > 0 && gamepadConnected == true)
                _remoteStateStore.RemoteConnected(true);

        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count() > 0 && gamepadConnected == false)
            {
                gamepadConnected = true;
                controller = Gamepad.Gamepads.First();
                StartPolling();
                _remoteStateStore.RemoteConnected(true);

            }
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count() == 0)
            {
                gamepadConnected = false;
                CancelPolling();
                _remoteStateStore.RemoteConnected(false);
            }
        }




        public void SendRemoteData(object source, ElapsedEventArgs e)
        {
            if (gamepadConnected)
            {
                GamepadReading gamepadReading = controller.GetCurrentReading();

                _remoteData.ProcessJoystickData(gamepadReading.RightThumbstickY, -gamepadReading.RightThumbstickX, -gamepadReading.LeftThumbstickY);

               _remoteStateStore.SendRemoteData(_remoteData);
            }
        }





        public void StartPolling()
        {
            this._cts = new CancellationTokenSource();
            Task.Factory.StartNew(this.ButtonPollingTask, this._cts.Token, TaskCreationOptions.LongRunning);
        }

        public void CancelPolling()
        {
            this._cts.Cancel();
        }

        /// <summary>
        /// "Infinite" loop that runs every N seconds. Good for checking for a heartbeat or updates.
        /// </summary>
        /// <param name="taskState">The cancellation token from our _cts field, passed in the StartNew call</param>
        private async void ButtonPollingTask(object taskState)
        {
            var token = (CancellationToken)taskState;

            BitArray ButtonsArrayPrevious = new BitArray(17);
            BitArray ButtonsArray;
            while (!token.IsCancellationRequested)
            {
                GamepadReading gamepadReading = controller.GetCurrentReading();

                 ButtonsArray = new BitArray(new int[] { (int)gamepadReading.Buttons });

                if (ButtonsArray[1] && !ButtonsArrayPrevious[1])
                  {
                    _remoteData.ToggleARM();

                }

                if (ButtonsArray[4] && !ButtonsArrayPrevious[4])
                {
                    _remoteData.TogglePositionHolding();
                }

                if (ButtonsArray[2] && !ButtonsArrayPrevious[2])
                {
                    _remoteData.DecreaseLights();
                }

                if (ButtonsArray[3] && !ButtonsArrayPrevious[3])
                {
                    _remoteData.IncreaseLights();
                }

                if (ButtonsArray[5] && !ButtonsArrayPrevious[5])
                {
                    _remoteData.HomeServo();
                }

                if (ButtonsArray[6] && !ButtonsArrayPrevious[6])
                {
                    _remoteData.IncreaseTServo();
                }
                if (ButtonsArray[7] && !ButtonsArrayPrevious[7])
                {
                    _remoteData.DecreaseTServo();
                }
                if (ButtonsArray[8] && !ButtonsArrayPrevious[8])
                {
                    _remoteData.IncreasePServo();
                }
                if (ButtonsArray[9] && !ButtonsArrayPrevious[9])
                {
                    _remoteData.DecreasePServo();
                }
                if (ButtonsArray[10] && !ButtonsArrayPrevious[10])
                {
                    _remoteData.DecreaseGear();
                }
                if (ButtonsArray[11] && !ButtonsArrayPrevious[11])
                {
                    _remoteData.IncreaseGear();
                }

                /*switch (gamepadReading.Buttons)
                {

                    case GamepadButtons.DPadUp:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons || ContinueClick)
                        {
                            _remoteData.IncreaseTServo();
                            ContinueClick = false;
                        }
                        break;

                    case GamepadButtons.DPadDown:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons || ContinueClick)
                        {
                            _remoteData.DecreaseTServo();
                            ContinueClick = false;
                        }
                        break;

                    case GamepadButtons.DPadRight:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons || ContinueClick)
                        {
                            _remoteData.DecreasePServo();
                            ContinueClick = false;
                        }
                        break;

                    case GamepadButtons.DPadLeft:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons || ContinueClick)
                        {
                            _remoteData.IncreasePServo();
                            ContinueClick = false;
                        }
                        break;

                    case GamepadButtons.RightShoulder:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                        {
                            _remoteData.IncreaseGear();
                        }
                        break;

                    case GamepadButtons.LeftShoulder:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                            _remoteData.DecreaseGear();
                        break;

                    case GamepadButtons.View:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                            _remoteData.ToggleARM(); // 
                        break;

                    case GamepadButtons.Y:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                            _remoteData.HomeServo(); //
                        break;

                    case GamepadButtons.X:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                            _remoteData.TogglePositionHolding(); //
                        break;

                    case GamepadButtons.A:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                        {
                            _remoteData.DecreaseLights(); //
                        }
                        break;

                    case GamepadButtons.B:
                        if (PreviousButtonPressed != (int)gamepadReading.Buttons)
                        {
                            _remoteData.IncreaseLights(); //
                        }
                        break;
                } */


                ButtonsArrayPrevious = ButtonsArray;
                // Debug.WriteLine("*********************");
                // Passing token here allows the Delay to be cancelled if your task gets cancelled.
                await Task.Delay(TASK_ITERATION_DELAY_MS);

            }
        }
    }

}
