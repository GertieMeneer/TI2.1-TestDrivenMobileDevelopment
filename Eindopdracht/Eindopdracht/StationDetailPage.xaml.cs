using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
    public Database _database;

    public StationDetailPage(NSStation station, Location currentLocation)
    {
        InitializeComponent();
        _station = station;
        _currentLocation = currentLocation;
        _database = new Database();
        Title = $"{station.Naam} - Details";
        BindingContext = new StationDetailViewModel(station);
        Load();
    }

    private bool CheckIfInFavourites()
    {
        List<Station> stations = _database.GetStations();

        if (stations.Count.Equals(0)) { return false; }

        foreach (Station station in stations)
        {
            if (station.Naam == _station.Naam)
            {
                return true; 
            }
        }
        return false;
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

        var currentLocationCircle = new Circle
        {
            Center = new Location(_currentLocation.Latitude, _currentLocation.Longitude),
            Radius = Distance.FromMeters(100),
            StrokeColor = Colors.White,
            StrokeWidth = 8,
            FillColor = Colors.Green,
        };

        var stationCircle = new Circle
        {
            Center = new Location(_station.Lat, _station.Lng),
            Radius = Distance.FromMeters(100),
            StrokeColor = Colors.White,
            StrokeWidth = 8,
            FillColor = Colors.Green,
        };

        var line = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 15,
            Geopath =
            {
                new Location(_station.Lat, _station.Lng),
                new Location(_currentLocation.Latitude, _currentLocation.Longitude),
            }
        };

        map.MapElements.Add(line);

        map.MapElements.Add(currentLocationCircle);
        map.MapElements.Add(stationCircle);

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

    private Station GetStation(NSStation NSStation)
    {
        Station station = new Station
        {
            Naam = NSStation.Naam,
            Distance = NSStation.Distance,
            Lat = NSStation.Lat,
            Lng = NSStation.Lng
        };

        return station;
    }

    private void FavoritesButton_OnClicked(object? sender, EventArgs e)
    {
        if (!CheckIfInFavourites())
        {
            _database.SaveStation(GetStation(_station));
            showNotification(5, "Eindopdracht", "Added to favorites.");
            Toast.Make("Added to favorites.", ToastDuration.Long, 15);
        }
        else
        {
            _database.DeleteStationByName(_station.Naam);
            showNotification(5, "Eindopdracht", "Removed from favorites.");
            Toast.Make("Removed from favorites.", ToastDuration.Long, 15);
        }
    }
}