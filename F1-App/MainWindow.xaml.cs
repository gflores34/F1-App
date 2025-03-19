using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace F1_App
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private string _sessionKey = "latest";
        private DispatcherTimer? _lapTimer;
        private int _currentLap;
        private string _sessionName = "";
        #endregion

        #region Properties

        public string SessionKey
        {
            get => _sessionKey;
            set
            {
                if (_sessionKey != value)
                {
                    _sessionKey = value;
                    NotifyPropertyChanged(nameof(SessionKey));
                    Task.Run(() => LoadDrivers()); // Use Task.Run
                }
            }
        }

        public string SessionName
        {
            get => _sessionName;
            set
            {
                if (_sessionName != value)
                {
                    _sessionName = value;
                    NotifyPropertyChanged(nameof(SessionName));
                }
            }
        }

        public ObservableCollection<Driver> Drivers { get; set; } = new ObservableCollection<Driver>();

        public DispatcherTimer? LapTimer
        {
            get => _lapTimer;
            set
            {
                if (_lapTimer != value)
                {
                    _lapTimer = value;
                    NotifyPropertyChanged(nameof(LapTimer));
                }
            }
        }

        public int CurrentLap
        {
            get => _currentLap;
            set
            {
                _currentLap = value;
                NotifyPropertyChanged(nameof(CurrentLap));
            }
        }

        private readonly ApiService _apiService = new ApiService();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            InitializeLapTimer();
            DriverList.ItemsSource = Drivers;
            ControlWindow controlWindow = new ControlWindow();
            controlWindow.SessionKeyChanged += ControlWindow_SessionKeyChanged;
        }

        #endregion

        #region Event Handlers

        private void ShowControlWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ControlWindow controlWindow = new ControlWindow();
            controlWindow.Show();
        }

        private void ShowSettingsWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private async void OpenRaceInfoButton_Click(Object sender, RoutedEventArgs e)
        {
            RaceInfoWindow raceInfoWindow = new RaceInfoWindow(_apiService, SessionKey);
            raceInfoWindow.Owner = this;
            raceInfoWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            await raceInfoWindow.LoadSessionData();
            raceInfoWindow.ShowDialog();

            
        }

        private void ControlWindow_SessionKeyChanged(string newSessionKey)
        {
            SessionKey = newSessionKey;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDrivers();
        }

        private async void LapTimer_Tick(object? sender, EventArgs e)
        {
            Debug.WriteLine("LapTimer_Tick called");
            try
            {
                CurrentLap = await _apiService.GetCurrentLapForLeaderAsync(SessionKey);
                var positionsTask = _apiService.GetPositionsAsync(SessionKey);
                var lapsTask = _apiService.GetLapsAsync(SessionKey);

                await Task.WhenAll(positionsTask, lapsTask);

                List<Position>? positions = await positionsTask;
                List<Lap>? laps = await lapsTask;

                if (positions != null && Drivers != null && laps != null)
                {
                    var groupedPositions = positions.GroupBy(p => p.DriverNumber);
                    foreach (Driver driver in Drivers)
                    {
                        var latestPosition = groupedPositions.FirstOrDefault(g => g.Key == driver.DriverNumber)?.LastOrDefault();
                        if (latestPosition != null)
                        {
                            driver.Position = latestPosition.DriverPosition;
                        }

                        // Check for DNF using lap data
                        var driverLaps = laps.Where(l => l.DriverNumber == driver.DriverNumber);
                        int lastDriverLap = driverLaps.Any() ? driverLaps.Max(l => l.LapNumber) : 0; // Get driver's last lap

                        driver.IsDNF = CurrentLap - lastDriverLap > 2; // Simplified DNF check
                    }
                    await UpdateIntervals();
                    await LoadStints();
                    UpdateDriverOrder();
                }
                else
                {
                    Debug.WriteLine("Could not retrieve positions or driver data.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating driver data: {ex.Message}");
                // Consider more user-friendly error display or logging
            }
        }

        #endregion

        #region Data Loading and Updating

        private async Task LoadDrivers()
        {
            try
            {
                List<Session>? sessions = await _apiService.GetSessionAsync(SessionKey);

                if (sessions.First().SessionName != null && sessions.Any())
                {
                    string sessionName = sessions.First().SessionName ?? "NULL";
                    if (sessionName == "Race") 
                    {
                        SessionName = "RACE";
                    }
                    else if(sessionName == "Sprint")
                    {
                        SessionName = "SPRINT";
                    }
                    else if(sessionName == "Qualifying")
                    {
                        SessionName = "QUALI";
                    }
                }
                else
                {
                    Debug.WriteLine("No sessions found or sessions list was null.");
                    SessionName = "Default Session Name"; // Or handle the absence of a session as needed
                }

                var driversTask = _apiService.GetDriversAsync(SessionKey);
                var positionsTask = _apiService.GetPositionsAsync(SessionKey);

                await Task.WhenAll(driversTask, positionsTask);

                List<Driver>? driversList = await driversTask;
                List<Position>? positions = await positionsTask;

                if (driversList != null && positions != null)
                {
                    Drivers.Clear();
                    foreach (var driver in driversList)
                    {
                        Drivers.Add(driver);
                    }

                    var groupedPositions = positions.GroupBy(p => p.DriverNumber);

                    foreach (Driver driver in Drivers)
                    {
                        var latestPosition = groupedPositions.FirstOrDefault(g => g.Key == driver.DriverNumber)?.LastOrDefault();
                        if (latestPosition != null)
                        {
                            driver.Position = latestPosition.DriverPosition;
                        }
                    }

                    UpdateDriverOrder();
                }
                else
                {
                    MessageBox.Show($"Could not retrieve driver or position data. Session Key: {SessionKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error: {ex.Message}. Session Key: {SessionKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Data parsing error: {ex.Message}. Session Key: {SessionKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}. Session Key: {SessionKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadStints()
        {
            try
            {
                List<Stint>? stints = await _apiService.GetStintsAsync(SessionKey);
                if (stints != null && Drivers != null)
                {
                    foreach (var driver in Drivers)
                    {
                        driver.Stints = stints.Where(s => s.DriverNumber == driver.DriverNumber).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading stints: {ex.Message}");
            }
        }

        private async Task UpdateIntervals()
        {
            try
            {
                var intervalsTask = _apiService.GetIntervalsAsync(SessionKey);
                var lapsTask = _apiService.GetLapsAsync(SessionKey);

                await Task.WhenAll(intervalsTask, lapsTask);

                List<Interval>? intervals = await intervalsTask;
                List<Lap>? laps = await lapsTask;

                if (intervals != null && Drivers != null && laps != null)
                {
                    var groupedIntervals = intervals.GroupBy(i => i.DriverNumber);

                    if (groupedIntervals != null)
                    {
                        foreach (Driver driver in Drivers)
                        {
                            var latestInterval = groupedIntervals.FirstOrDefault(g => g.Key == driver.DriverNumber)?.LastOrDefault();

                            if (latestInterval != null)
                            {
                                int currentLap = laps.Where(l => l.DriverNumber == driver.DriverNumber).Max(l => l.LapNumber);
                                var currentStint = driver.Stints?.FirstOrDefault(s => currentLap >= s.LapStart && currentLap <= s.LapEnd);

                                // Check for DNF status first
                                if (driver.IsDNF)
                                {
                                    driver.CurrentTireCompound = "-";
                                    driver.DisplayValue = "DNF"; // Consistent DNF display
                                }
                                else
                                {
                                    // Only calculate tire compound if not DNF
                                    driver.CurrentTireCompound = string.IsNullOrEmpty(currentStint?.Compound)
                                        ? "-"
                                        : currentStint.Compound.Substring(0, 1).ToUpper();

                                    // Determine DisplayValue based on GapSetting
                                    if (Properties.Settings.Default.GapSetting == "Gap to Leader")
                                    {
                                        driver.DisplayValue = latestInterval.GapToLeader == null
                                            ? "-"
                                            : latestInterval.GapToLeader == "0.0" || latestInterval.GapToLeader == "0"
                                            ? "Leader"
                                            : "+" + latestInterval.GapToLeader;
                                    }
                                    else
                                    {
                                        driver.DisplayValue = latestInterval.IntervalTime == null
                                            ? "-"
                                            : latestInterval.GapToLeader == "0.0" || latestInterval.GapToLeader == "0"
                                                ? "Leader"
                                                : "+" + latestInterval.IntervalTime;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Grouped intervals are null.");
                    }
                }
                else
                {
                    Debug.WriteLine("Could not retrieve interval data or laps data.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating interval data: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private void InitializeLapTimer()
        {
            LapTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };
            LapTimer.Tick += LapTimer_Tick;
            LapTimer.Start();
        }

        public void RefreshDriverList()
        {
            foreach (var driver in Drivers)
            {
                driver.DisplayValue = Properties.Settings.Default.GapSetting == "Gap to Leader" ? driver.GapToLeader : driver.IntervalTime;
            }
            DriverList.Items.Refresh();
        }

        private void UpdateDriverOrder()
        {
            var sortedDrivers = Drivers
                .OrderBy(driver => driver.IsDNF) // DNF drivers last
                .ThenBy(driver => driver.Position) // Then sort by position
                .ToList();
            Drivers.Clear();
            foreach (Driver driver in sortedDrivers)
            {
                Drivers.Add(driver);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}