using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Session
    {
        
        [JsonProperty("circuit_key")]
        public int CircuitKey { get; set; }

        [JsonProperty("circuit_short_name")]
        public string? CircuitShortName { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("country_key")]
        public int CountryKey { get; set; }

        [JsonProperty("country_name")]
        public string? CountryName { get; set; }

        [JsonProperty("gmt_offset")]
        public string? GmtOffset { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("meeting_key")]
        public int MeetingKey { get; set; }

        [JsonProperty("session_key")]
        public int SessionKey { get; set; }

        [JsonProperty("session_name")]
        public string? SessionName { get; set; } = "RACE";
        
        [JsonProperty("session_type")]
        public string? SessionType { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
        
    }
}
