using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Stint
    {
        [JsonProperty("session_key")]
        public int SessionKey { get; set; }

        [JsonProperty("driver_number")]
        public int DriverNumber { get; set; }

        [JsonProperty("lap_start")]
        public int LapStart { get; set; }

        [JsonProperty("lap_end")]
        public int LapEnd { get; set; }

        [JsonProperty("compound")]
        public string? Compound { get; set; }

    }
}
