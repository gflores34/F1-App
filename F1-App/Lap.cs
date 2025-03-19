using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Lap
    {
        [JsonProperty("lap_number")]
        public int LapNumber { get; set; }

        [JsonProperty("driver_number")]
        public required int DriverNumber { get; set; }

    }


}
