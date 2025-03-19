using System.Diagnostics;
using System.Windows;

namespace F1_App
{
    public partial class RaceInfoWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();

        public string SessionKey { get; set; }

        public RaceInfoWindow(ApiService apiService, string sessionKey)
        {
            InitializeComponent();
            _apiService = apiService;
            SessionKey = sessionKey;
        }


        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }

        public async Task LoadSessionData()
        {
            try
            {
                // Call your API service to get session info
                var sessionTask = _apiService.GetSessionAsync(SessionKey);
                var weatherTask = _apiService.GetWeatherAsync(SessionKey);

                var sessionInfo = await sessionTask;
                var weatherInfo = await weatherTask;

                //Debug.WriteLine($"WeatherInfo count: {weatherInfo.Count}");

                if (sessionInfo != null && weatherInfo != null)
                {
                    // Update labels with fetched data
                    SessionNameValue.Text = "" + sessionInfo[0].SessionName;
                    LocationValue.Text = "" + sessionInfo[0].Location;
                    CountryValue.Text = "" + sessionInfo[0].CountryName;
                    TrackTempValue.Text = "" + weatherInfo[0].TrackTemp + "°C";
                    TrackValue.Text = "" + sessionInfo[0].CircuitShortName;
                    PressureValue.Text = "" + weatherInfo[weatherInfo.Count - 1].AirPressure + " mbar";
                    HumidityValue.Text = "" + weatherInfo[weatherInfo.Count - 1].Humidity + "%";
                    WindSpeedValue.Text = "" + weatherInfo[weatherInfo.Count - 1].WindSpeed + " m/s";
                    AirTempLabel.Content = weatherInfo[weatherInfo.Count - 1].AirTemp + "°C";
                    
                }
                else
                {
                    MessageBox.Show("Failed to retrieve session information.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}