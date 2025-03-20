# F1 App

This is a WPF application for displaying real-time Formula 1 race data. It fetches data from an external API, providing insights into driver positions, lap times, intervals, and more.

[f1-app](https://github.com/user-attachments/assets/9f63c981-bb4d-48af-bc9d-bb9e6f529a2f)

## Features!

-   **Real-time Driver Positions:** Displays the current positions of all drivers.
-   **Lap Times and Intervals:** Shows lap times and intervals between drivers or gap to the leader, updated periodically.
-   **Session Information:** Fetches and displays information about the current session (Race, Qualifying, etc.).
-   **Driver Stints:** Shows the stints (tire changes) for each driver.
-   **DNF (Did Not Finish) Tracking:** Indicates drivers who have retired from the race.
-   **Configurable Gap Setting:** Allows users to switch between displaying intervals and gap to the leader.
-   **Race Information Window:** Provides detailed information about the race.

## Technologies Used

-   **WPF (.NET Framework):** For the user interface.
-   **C#:** For application logic.
-   **Newtonsoft.Json:** For JSON serialization and deserialization.
-   **System.Net.Http:** For making HTTP requests to the API.
-   **ObservableCollection:** For data binding and automatic UI updates.
-   **DispatcherTimer:** For periodic updates.
-   **INotifyPropertyChanged:** For data binding and UI updates.
-   **Task.Run and async/await:** For asynchronous operations and improved performance.

## Getting Started

### Prerequisites

-   [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework)

### Installation

1.  Clone the repository to your local machine:

    ```bash
    git clone https://github.com/gflores34/F1-App
    ```

2.  Open the solution file (`F1_App.sln`) in Visual Studio.
3.  Restore NuGet packages.
4.  Build and run the application.

## OpenF1 API

The application uses the **[OpenF1 API](https://openf1.org/)** to fetch race data. The `ApiService` class handles the HTTP requests and data parsing.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## License

[MIT](LICENSE)
