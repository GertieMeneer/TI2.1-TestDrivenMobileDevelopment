using Eindopdracht.NSData;
using Eindopdracht.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Plugin.LocalNotification;

namespace Eindopdracht;

public partial class StationDetailPage
{
    private NSStation _station;

    public StationDetailPage(NSStation station)
    {
        _station = station;
        InitializeComponent();
        Title = $"{station.Naam} - Details";
        BindingContext = new StationDetailViewModel(station);
        LocalNotificationCenter.Current.RequestNotificationPermission();
        Load();
    }

    private async void Load()
    {
        var location = new Location(_station.Lat, _station.Lng);

        double minLat = Math.Min(_station.Lat, location.Latitude);
        double minLng = Math.Min(_station.Lng, location.Longitude);
        double maxLat = Math.Max(_station.Lat, location.Latitude);
        double maxLng = Math.Max(_station.Lng, location.Longitude);

        double centerLat = (minLat + maxLat) / 2;
        double centerLng = (minLng + maxLng) / 2;

        double distanceKm = GetDistanceInKm(minLat, minLng, maxLat, maxLng);

        var mapSpan = MapSpan.FromCenterAndRadius(new Location(centerLat, centerLng), Distance.FromKilometers(distanceKm));

        var stationPin = new Pin
        {
            Label = "Station: " + _station.Naam,
            Location = new Location(_station.Lat, _station.Lng),
            Type = PinType.Place
        };

        double locationLat = Convert.ToDouble(Preferences.Get("locationLat", "error 404"));
        double locationLng = Convert.ToDouble(Preferences.Get("locationLng", "error 404"));

        var currentLocationPin = new Pin
        {
            Label = "Current location",
            Location = new Location(locationLat, locationLng),
            Type = PinType.Place
        };

        map.Pins.Add(stationPin);
        map.Pins.Add(currentLocationPin);

        map.MoveToRegion(mapSpan);

        await showNotification(5, "Eindopdracht", "Map loaded succesfully!");
    }

    private double GetDistanceInKm(double lat1, double lng1, double lat2, double lng2)
    {
        double R = 6371;
        double dLat = Deg2Rad(lat2 - lat1);
        double dLng = Deg2Rad(lng2 - lng1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                   Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;
        return distance;
    }

    private double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
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
}