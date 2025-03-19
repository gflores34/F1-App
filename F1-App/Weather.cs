using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Weather
    {
        [JsonProperty("air_temperature")]
        public float AirTemp { get; set; }

        [JsonProperty("humidity")]
        public float Humidity { get; set; }

        [JsonProperty("track_temperature")]
        public float TrackTemp { get; set; }

        [JsonProperty("pressure")]
        public float AirPressure { get; set; }

        [JsonProperty("wind_speed")]
        public float WindSpeed { get; set; }

    }
}
