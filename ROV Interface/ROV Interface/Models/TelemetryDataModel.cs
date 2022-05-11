using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Models
{
    public class TelemetryDataModel
    {

        public int Pitch { get; set; }
        public int Yaw { get; set; }
        public int Roll { get; set; }
        public int Depth { get; set; }
        public double Temperature { get; set; }

        public double Cell1 { get; set; }
        public double Cell2 { get; set; }
        public double Cell3 { get; set; }
        public double Cell4 { get; set; }
        public double Cell5 { get; set; }


        string[] separatingStrings = { "P", "Y", "R", "D", "T" };

        public TelemetryDataModel()
        {
        }

        public bool ProcessTelemetryData(string Data)
        {

            if (Data.StartsWith("P"))
            {
                string[] words = Data.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                Pitch = int.Parse(words[0]);
                Yaw = int.Parse(words[1]);
                Roll = int.Parse(words[2]);
                Depth = int.Parse(words[3]);
                Temperature = (double)int.Parse(words[4]) / 10;
                return true;
            }
            else
            {
                string[] words = Data.Split("B", System.StringSplitOptions.RemoveEmptyEntries);
                Cell1 = (double)int.Parse(words[0]) / 10;
                Cell2 = (double)int.Parse(words[1]) / 10;
                Cell3 = (double)int.Parse(words[2]) / 10;
                Cell4 = (double)int.Parse(words[3]) / 10;
                Cell5 = (double)int.Parse(words[4]) / 10;

                return false;
            }
        }
    }
}
