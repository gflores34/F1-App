using System;
using System.Windows;

namespace F1_App
{
    public partial class ControlWindow : Window
    {
        public ControlWindow()
        {
            InitializeComponent();
            SessionKeyChanged += delegate { };
        }

        public event Action<string> SessionKeyChanged;

        private void ApplySessionKeyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newSessionKey = SessionKeyTextBox.Text;
                SessionKeyChanged?.Invoke(newSessionKey);
                MessageBox.Show($"Session key set to: {newSessionKey}", "Session Key Applied");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApplySessionKeyButton_Click: {ex.Message}");
                MessageBox.Show($"Error applying session key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CloseApplicationButton_Click: {ex.Message}");
                MessageBox.Show($"Error closing application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}