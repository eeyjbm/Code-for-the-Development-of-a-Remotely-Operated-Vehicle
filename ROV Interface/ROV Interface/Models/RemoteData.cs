using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Models
{
    public class RemoteData
    {
        int gear1;
        int gear2;
        int gear3;

        public int Gear {  get; set; } //0-5
        public int ServoT { get; set; } // 0-18 *10
        public int ServoP { get; set; } // 0-22 *5+35

        public int Lights { get; set; } // 0-5

        public double Surge { get; set; } // 0-5
        public double Yaw { get; set; } // 0-5
        public double Heave { get; set; } // 0-5


        /* private double surge;
         public double Surge 
         {
             get
             {
                 return surge;
             }

             set
             {
                 switch (Gear) 
                 {
                     case 1:
                         surge = (value - -1) * ((gear1 / 100) - (-gear1 / 100)) / (1 - -1) + (-gear1/100);
                         break;
                     case 2:
                         surge = (value - -1) * ((gear2 / 100) - (-gear2 / 100)) / (1 - -1) + (-gear2 / 100);

                         break;
                     case 3:
                         surge = (value - -1) * ((gear3 / 100) - (-gear3 / 100)) / (1 - -1) + (-gear3/100);
                         break;
                 }

                 surge = (surge + 1)*499;
                 if (surge < 544 && surge > 454)
                     surge = 499;
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
                 switch (Gear)
                 {
                     case 1:
                         yaw = (value - -1) * ((gear1 / 100) - (-gear1 / 100)) / (1 - -1) + (-gear1 / 100);
                         break;
                     case 2:
                         yaw = (value - -1) * ((gear2 / 100) - (-gear2 / 100)) / (1 - -1) + (-gear2 / 100);

                         break;
                     case 3:
                         yaw = (value - -1) * ((gear3 / 100) - (-gear3 / 100)) / (1 - -1) + (-gear3 / 100);
                         break;
                 }
                 yaw = (yaw + 1)*499;
                 if (yaw < 544 && yaw > 454)
                     yaw = 499;
             }
         }  // left joy y

         private double heave;
         public double Heave
         {
             get
             {
                 return heave;
             }

             set
             {
                 switch (Gear)
                 {
                     case 1:
                         heave = (value - -1) * ((gear1 / 100) - (-gear1 / 100)) / (1 - -1) + (-gear1 / 100);
                         break;
                     case 2:
                         heave = (value - -1) * ((gear2 / 100) - (-gear2 / 100)) / (1 - -1) + (-gear2 / 100);

                         break;
                     case 3:
                         heave = (value - -1) * ((gear3 / 100) - (-gear3 / 100)) / (1 - -1) + (-gear3 / 100);
                         break;
                 }
                 heave = (heave + 1) * 499;
                 if (heave < 544 && heave > 454)
                     heave = 499;
             }  //right joy x
         }
        */
        public bool PositionHolding { get; set; } //B
        // x y menus 
        public bool ARM { get; set; }
        //menu settings 
        //triggers - graph time 
        public RemoteData()
        {
            Surge = 999;
            Yaw = 999;
            Heave = 999;
            Gear = 3;
            ServoT = 9;
            ServoP = 11;
            if(Properties.Settings.Default.LightsONOnStartUp)
            Lights = 5;
            else                
            Lights = 0;
            ARM = false;
            PositionHolding = Properties.Settings.Default.DepthHoldOnStartUp;
            gear1 = Properties.Settings.Default.Gear1;
            gear2 = Properties.Settings.Default.Gear2;
            gear3 = Properties.Settings.Default.Gear3;
            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
        }



        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            gear1 = Properties.Settings.Default.Gear1;
            gear2 = Properties.Settings.Default.Gear2;
            gear3 = Properties.Settings.Default.Gear3;

        }

        public void IncreaseGear()
        {
            if (Gear < 3)
                Gear++;
        }
        public void DecreaseGear()
        {
            if (Gear >1)
                Gear--;
        }

        public void IncreaseTServo() // *10
        {
            if(ARM)
            if (ServoT < 18)
                ServoT += 1;
        }
        public void DecreaseTServo()
        {
            if (ARM)
                if (ServoT > 0)
                ServoT -= 1;
        }

        public void IncreasePServo() //*5 + 35
        {
            if (ARM)
                if (ServoP < 22)
                ServoP += 1;
        }
        public void DecreasePServo() 
        {
            if (ARM)
                if (ServoP > 0)
                ServoP -= 1;
        }

        public void HomeServo() 
        {
            ServoT = 9;
            ServoP = 11;
        }

        public void IncreaseLights() //*20
        {
            if (Lights < 5)
                Lights ++;
        }
        public void DecreaseLights()
        {
            if (Lights > 0)
                Lights --;
        }

        public void ToggleARM()
        {
            ARM = !ARM; 
        }

        public void Disarm()
        {
            ARM = false;
        }

        public void TogglePositionHolding()
        {
            PositionHolding = !PositionHolding;
        }

        public void ProcessJoystickData(double S, double Y, double H )
        {


            if (S < 0.08 && S > -0.08)
                S = 0;

            if (Y < 0.08 && Y > -0.08)
                Y = 0;

            if (H < 0.08 && H > -0.08)
                H = 0;

            switch (Gear)
            {
                case 1:
                    Surge = (S + 1) * ((gear1 * 4.99 + 499) - (499 - gear1 * 4.99)) / (2) + (499 - gear1 * 4.99);
                    Yaw = (Y + 1) * ((gear1 * 4.99 + 499) - (499 - gear1 * 4.99)) / (2) + (499 - gear1 * 4.99);
                    Heave = (H + 1) * ((gear1 * 4.99 + 499) - (499 - gear1 * 4.99)) / (2) + (499 - gear1 * 4.99);

                    break;
                case 2:
                    Surge = (S+1) * ((gear2 * 4.99 + 499) - (499 - gear2 * 4.99)) / (2) + (499 - gear2 * 4.99);
                    Yaw = (Y + 1) * ((gear2 * 4.99 + 499) - (499 - gear2 * 4.99)) / (2) + (499 - gear2 * 4.99);
                    Heave = (H + 1) * ((gear2 * 4.99 + 499) - (499-gear2 * 4.99)) / (2) + (499 - gear2 * 4.99);

                    break;
                case 3:
                    Surge = (S+1) * ((gear3 * 4.99 + 499) - (499 - gear3 * 4.99)) / (2) + (499 - gear3 * 4.99);
                    Yaw = (Y + 1) * ((gear3 * 4.99 + 499) - (499 - gear3 * 4.99)) / (2) + (499 - gear3 * 4.99);
                    Heave = (H + 1) * ((gear3 * 4.99 + 499) - (499 - gear3 * 4.99)) / (2) + (499 - gear3 * 4.99);

                    break;
            }


        }

    }
}
