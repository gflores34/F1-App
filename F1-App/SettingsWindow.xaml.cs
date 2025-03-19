using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace F1_App
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            // Load settings
            if (Properties.Settings.Default.GapSetting == "Gap to Leader")
            {
                DisplayComboBox.SelectedIndex = 0;
            }
            else
            {
                DisplayComboBox.SelectedIndex = 1;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Save settings
            string displaySetting = (DisplayComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Null check and default value
            if (string.IsNullOrEmpty(displaySetting))
            {
                displaySetting = "Gap to Leader"; // Or any appropriate default value
            }

            Properties.Settings.Default.GapSetting = displaySetting;
            Properties.Settings.Default.Save();

            // Notify the main window to refresh
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.RefreshDriverList();
            }

            this.Close();
        }
    }
}