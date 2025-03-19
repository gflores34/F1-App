using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace F1_App
{
    public class Driver : INotifyPropertyChanged
    {
        #region Properties

        [JsonProperty("name_acronym")]
        public required string NameAcronymn { get; set; }

        [JsonProperty("driver_number")]
        public required int DriverNumber { get; set; }

       [JsonProperty("team_name")]
        public string TeamName { get; set; } = string.Empty; // Initialize with a default value

        // TeamLogo - Managed within the application
        public string TeamLogo { get; set; } = string.Empty; // Initialize with a default value

        public int Position { get; set; }

        public int LapNumber { get; set; }

        public List<Stint> Stints { get; set; } = new List<Stint>();

        [JsonProperty("interval")]
        public required string IntervalTime { get; set; }

        [JsonProperty("gap")]
        public required string GapToLeader { get; set; }

        private string _displayValue = "-";
        public string DisplayValue
        {
            get => _displayValue;
            set
            {
                if (_displayValue != value)
                {
                    _displayValue = value;
                    OnPropertyChanged(nameof(DisplayValue));
                }
            }
        }

        private string _currentTireCompound = "";
        public string CurrentTireCompound
        {
            get => _currentTireCompound;
            set
            {
                if (_currentTireCompound != value)
                {
                    _currentTireCompound = value;
                    CurrentTireCompound = value;
                    OnPropertyChanged(nameof(CurrentTireCompound));
                    UpdateTireCompoundColor();
                }
            }
        }

        private Brush _tireCompoundColor = Brushes.Gray;
        public Brush TireCompoundColor
        {
            get => _tireCompoundColor;
            set
            {
                if (_tireCompoundColor != value)
                {
                    _tireCompoundColor = value;
                    OnPropertyChanged(nameof(TireCompoundColor));
                }
            }
        }

        private bool _isDNF;
        public bool IsDNF
        {
            get => _isDNF;
            set
            {
                if (_isDNF != value)
                {
                    _isDNF = value;
                    OnPropertyChanged(nameof(IsDNF));
                }
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateTireCompoundColor()
        {
            TireCompoundColor = CurrentTireCompound?.ToUpper() switch
            {
                "S" => Brushes.Red,
                "M" => Brushes.Yellow,
                "H" => Brushes.White,
                "I" => Brushes.Green,
                "W" => Brushes.Blue,
                _ => Brushes.Gray,
            };
        }



        #endregion
    }

    public class DriverResponse
    {
        public required List<Driver> Drivers { get; set; }
    }

}