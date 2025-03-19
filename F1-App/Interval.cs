using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Interval
    {

        [JsonProperty("driver_number")]
        public int DriverNumber { get; set; }

        [JsonProperty("gap_to_leader")]
        public string? GapToLeader { get; set; }

        [JsonProperty("interval")]
        public string? IntervalTime { get; set; }


    }


}
