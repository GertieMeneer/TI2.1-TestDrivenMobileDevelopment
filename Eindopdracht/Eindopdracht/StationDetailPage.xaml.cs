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

    private Polyline line;

    private Circle currentLocationCircle;
    private Pin currentLocationPin;

    private Circle stationCircle;
    private Pin stationPin;

    public StationDetailPage(NSStation station, Location currentLocation)
    {
        InitializeComponent();
        _station = station;
        _currentLocation = currentLocation;
        Title = $"{station.Namen.Lang} - Details";
        BindingContext = new StationDetailViewModel(station);
        Load();
        if (CheckIfInFavourites())
        {
            FavoritesButton.Text = "Remove from favourites";
        }
        OnStartListening();
    }

    private bool CheckIfInFavourites()
    {
        List<DatabaseStation> stations = Database.GetFavouriteStations();

        if (stations.Count.Equals(0)) { return false; }

        foreach (DatabaseStation station in stations)
        {
            if (station.Naam == _station.Namen.Lang)
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

        stationPin = new Pin
        {
            Label = "Station: " + _station.Namen.Lang,
            Location = new Location(_station.Lat, _station.Lng),
            Type = PinType.Place
        };

        currentLocationPin = new Pin
        {
            Label = "Current location",
            Location = new Location(_currentLocation.Latitude, _currentLocation.Longitude),
            Type = PinType.Place
        };

        currentLocationCircle = new Circle
        {
            Center = new Location(_currentLocation.Latitude, _currentLocation.Longitude),
            Radius = Distance.FromMeters(100),
            StrokeColor = Colors.White,
            StrokeWidth = 8,
            FillColor = Colors.Green,
        };

        stationCircle = new Circle
        {
            Center = new Location(_station.Lat, _station.Lng),
            Radius = Distance.FromMeters(100),
            StrokeColor = Colors.White,
            StrokeWidth = 8,
            FillColor = Colors.Green,
        };

        line = new Polyline
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

    private void updateCurrentLocationElements(double lat, double lng)
    {
        line = null;
        currentLocationCircle = null;
        currentLocationPin = null;

        line = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 15,
            Geopath =
            {
                new Location(_station.Lat, _station.Lng),
                new Location(lat, lng),
            }
        };

        currentLocationPin = new Pin
        {
            Label = "Current location",
            Location = new Location(lat, lng),
            Type = PinType.Place
        };

        currentLocationCircle = new Circle
        {
            Center = new Location(lat, lng),
            Radius = Distance.FromMeters(100),
            StrokeColor = Colors.White,
            StrokeWidth = 8,
            FillColor = Colors.Green,
        };

        map.MapElements.Add(line);
        map.MapElements.Add(currentLocationCircle);
        map.Pins.Add(currentLocationPin);
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

    private DatabaseStation GetStation(NSStation NSStation)
    {
        DatabaseStation station = new DatabaseStation
        {
            Id = NSStation.Id,
            Naam = NSStation.Namen.Lang,
            Distance = NSStation.Distance,
            Lat = NSStation.Lat,
            Lng = NSStation.Lng,
            StationType = NSStation.StationType,
            HeeftFaciliteiten = NSStation.HeeftFaciliteiten,
            HeeftReisassistentie = NSStation.HeeftReisassistentie,
            Land = NSStation.Land
        };

        return station;
    }

    private void FavoritesButton_OnClicked(object? sender, EventArgs e)
    {
        if (!CheckIfInFavourites())
        {
            Database.SaveFavouriteStation(GetStation(_station));
            showNotification(0, "Eindopdracht", "Added to favorites.");
            FavoritesButton.Text = "Remove from favourites";
        }
        else
        {
            Database.DeleteFavouriteStationByName(_station.Namen.Lang);
            showNotification(0, "Eindopdracht", "Removed from favorites.");
            FavoritesButton.Text = "Add to favourites";
        }
    }

    private static double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
    {
        double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
        return distance;
    }

    async void OnStartListening()
    {
        try
        {
            Geolocation.LocationChanged += Geolocation_LocationChanged;
            var request = new GeolocationListeningRequest(GeolocationAccuracy.Default);
            var success = await Geolocation.StartListeningForegroundAsync(request);

            string status = success
                ? "Started listening for foreground location updates"
                : "Couldn't start listening";
        }
        catch (Exception ex)
        {
            // Unable to start listening for location changes
        }
    }

    async void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
    {
        map.MapElements.Clear();
        map.Pins.Clear();
        map.MapElements.Add(stationCircle);
        map.Pins.Add(stationPin);

        updateCurrentLocationElements(e.Location.Latitude, e.Location.Longitude);

        var distance = CalculateDistance(e.Location.Latitude, e.Location.Longitude, _station.Lat, _station.Lng);
        if (distance < 0.15) 
        { 
            await showNotification(5, "Eindopdracht", "You are very close to the station."); 
        }
    }
}