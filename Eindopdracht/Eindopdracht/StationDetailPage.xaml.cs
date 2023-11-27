using Eindopdracht.NSData;
using Eindopdracht.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Plugin.LocalNotification;

namespace Eindopdracht;

public partial class StationDetailPage
{
    private NSStation _station;
    private Location _currentLocation;

    public StationDetailPage(NSStation station, Location currentLocation)
    {
        _station = station;
        _currentLocation = currentLocation;
        InitializeComponent();
        Title = $"{station.Naam} - Details";
        BindingContext = new StationDetailViewModel(station);
        Load();
    }

    private async void Load()
    {
        var location = new Location(_station.Lat, _station.Lng);

        var mapSpan = new MapSpan(location, 0.01, 00.1);

        var stationPin = new Pin
        {
            Label = "Station: " + _station.Naam,
            Location = new Location(_station.Lat, _station.Lng),
            Type = PinType.Place
        };

        var currentLocationPin = new Pin
        {
            Label = "Current location",
            Location = new Location(_currentLocation.Latitude, _currentLocation.Longitude),
            Type = PinType.Place
        };

        map.Pins.Add(stationPin);
        map.Pins.Add(currentLocationPin);

        map.MoveToRegion(mapSpan);

        await showNotification(5, "Eindopdracht", "Map loaded succesfully!");
    }

    private async Task showNotification(int whenSeconds, string title, string description)
    {
        var request = new NotificationRequest
        {
            Title = title,
            Description = description,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(whenSeconds)
                //NotifyRepeatInterval = TimeSpan.FromSeconds(10),
            }
        };
        LocalNotificationCenter.Current.Show(request);
    }

    private void FavoritesButton_OnClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}