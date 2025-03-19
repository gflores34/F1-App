using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    public class Position
    {
        [JsonProperty("driver_number")]
        public required int DriverNumber { get; set; }

        [JsonProperty("position")]
        public int DriverPosition {  get; set; }
    }


}


