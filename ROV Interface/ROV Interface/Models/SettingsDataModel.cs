using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Models
{
    public class SettingsDataModel
    {

        public int PTerm { get; set; } 
        public int DTerm { get; set; } 
        public int ITerm { get; set; } 
        public int MaxThrusterChange { get; set; } 
        public int WaterDensity { get; set; }
        public bool RecoveryON { get; set; } //0-5
        public int RecoveryTimeDelay { get; set; } //0-5
        public int RecoveryThrustorPower { get; set; } //0-5


        public SettingsDataModel()
        { }

        public void SetSettingsModel()
        {

            PTerm = (int) (Properties.Settings.Default.PTerm * 1000);
            DTerm = (int)(Properties.Settings.Default.DTerm * 1000);
            ITerm = (int)(Properties.Settings.Default.ITerm * 1000);
            MaxThrusterChange = Properties.Settings.Default.MaxThrusterChange;
            WaterDensity = Properties.Settings.Default.FluidDensity;
            RecoveryON = Properties.Settings.Default.RecoveryON;
            RecoveryTimeDelay = Properties.Settings.Default.RecoveryTimeDelay;
            RecoveryThrustorPower = Properties.Settings.Default.RecoveryThrustorPower;


        }
    }
}
