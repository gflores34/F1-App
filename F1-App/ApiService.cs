using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace F1_App
{
    public class ApiService
    {
        private readonly string _baseApiUrl = "https://api.openf1.org/";

        public ApiService() { } // Add a constructor

        private async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(jsonResponse);
                    }
                    else
                    {
                        Debug.WriteLine($"API request to {_baseApiUrl + endpoint} failed with status code: {response.StatusCode}");
                        return default; // Or throw an exception if you prefer
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP request error: {ex.Message}");
                return default;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON deserialization error: {ex.StackTrace}");
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
                return default;
            }
        }


        public async Task<List<Driver>> GetDriversAsync(string sessionKey)
        {
            List<Driver>? drivers = await GetAsync<List<Driver>>($"v1/drivers?session_key={sessionKey}");

            if (drivers != null)
            {
                foreach (Driver driver in drivers)
                {
                    driver.TeamLogo = TeamLogoHelper.GetTeamLogo(driver.TeamName);
                }
                return drivers;
            }
            else
            {
                return new List<Driver>();
            }
        }

        public async Task<List<Position>> GetPositionsAsync(string sessionKey)
        {
            List<Position>? positions = await GetAsync<List<Position>>($"v1/position?session_key={sessionKey}");
            return positions ?? new List<Position>();
        }


        public async Task<int> GetCurrentLapForLeaderAsync(string sessionKey)
        {
            try
            {
                // Get the current positions
                List<Position> positions = await GetPositionsAsync(sessionKey);
                Console.WriteLine($"Positions Count: {positions?.Count}");
                if (positions == null || positions.Count == 0)
                {
                    Console.WriteLine("Could not retrieve positions.");
                    return 0;
                }

                // Assuming the first element in the list is the leader (check your API's behavior)
                int leaderDriverNumber = positions[0].DriverNumber;
                Console.WriteLine($"Leader Driver Number: {leaderDriverNumber}"); // Logging

                // Get laps for the leader
                List<Lap> laps = await GetLapsAsync(sessionKey, leaderDriverNumber);


                if (laps == null || laps.Count == 0)
                {
                    Console.WriteLine("Could not retrieve laps for leader.");
                    return 0;
                }

                // **Corrected Lap Identification: Get the last lap**
                int currentLap = 0;
                if (laps.Any())
                {
                    // **Important: Ensure laps are ordered correctly (e.g., by timestamp)**
                    // **If not, you MUST sort them before taking the last one.**

                    // **Assuming laps are already ordered by time in the API response**
                    currentLap = laps.Last().LapNumber;

                    // **If laps are NOT ordered, you need to sort them by a timestamp or other appropriate field:**
                    //currentLap = laps.OrderBy(lap => lap.TimestampField).Last().LapNumber; // Replace TimestampField
                }

                return currentLap;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            return 0; // Or handle errors appropriately
        }

        public async Task<List<Interval>> GetIntervalsAsync(string sessionKey)
        {
            List<Interval>? intervals = await GetAsync<List<Interval>>($"v1/intervals?session_key={sessionKey}");
            return intervals ?? new List<Interval>(); // Return an empty list if null
        }

        public async Task<List<Stint>> GetStintsAsync(string sessionKey)
        {
            List<Stint>? stints = await GetAsync<List<Stint>>($"v1/stints?session_key={sessionKey}");
            return stints ?? new List<Stint>();
        }

        public async Task<List<Lap>> GetLapsAsync(string sessionKey)
        {
            List<Lap> ? laps = await GetAsync<List<Lap>>($"v1/laps?session_key={sessionKey}");
            return laps ?? new List<Lap>();
        }

        public async Task<List<Lap>> GetLapsAsync(string sessionKey, int driverNumber)
        {

            List<Lap>? laps = await GetAsync<List<Lap>>($"v1/laps?session_key={sessionKey}&driver_number={driverNumber}");
            return laps ?? new List<Lap>();
        }

        public async Task<List<Session>> GetSessionAsync(string sessionKey)
        {
            List<Session>? session = await GetAsync<List<Session>>($"v1/sessions?session_key={sessionKey}");
            return session ?? new List<Session>();
        }

        public async Task<List<Weather>> GetWeatherAsync(string sessionKey)
        {
            List<Weather>? weather = await GetAsync<List<Weather>>($"v1/weather?session_key={sessionKey}");
            return weather ?? new List<Weather>();
        }
    }
}