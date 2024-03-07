using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Eindopdracht.Interfaces;
using Eindopdracht.NSData;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using Plugin.LocalNotification;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Eindopdracht.ViewModels
{
    [QueryProperty(nameof(Station), "s")]
    public partial class StationDetailViewModel : ObservableObject
    {
        private Location _location;

        private Polyline line;

        private Circle currentLocationCircle;
        private Pin currentLocationPin;

        private Circle stationCircle;
        private Pin stationPin;

        private string buttonText;

        private Map map;

        private IDatabase _database;

        public StationDetailViewModel(IDatabase database) { _database = database; }

        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Loads and sets elements on detail page
        /// </summary>
        /// <param name="map">Map of the device and station</param>
        /// <returns></returns>
        public async Task Initialize(Map map)
        {
            this.map = map;

            GetCurrentLocationAndSetIt();

            // waiting for ObservableProperty Station to be loaded
            await Task.Delay(4500);

            Load();

            if (CheckIfInFavourites())
            {
                ButtonText = "Remove from favourites";
            }
            else
            {
                ButtonText = "Add to favourites";
            }

            OnStartListening();
        }

        /// <summary>
        /// Checks if a station is inside the favourites database
        /// </summary>
        /// <returns>True if it is in the database, otherwise false</returns>
        private bool CheckIfInFavourites()
        {
            List<DatabaseStation> stations = _database.GetFavouriteStations();

            if (stations.Count.Equals(0)) { return false; }

            foreach (DatabaseStation station in stations)
            {
                if (station.Naam == Station.Namen.Lang)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets and sets the current device location
        /// </summary>
        /// <returns></returns>
        public async Task GetCurrentLocationAndSetIt()
        {
            try
            {
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                _location = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //When location is not supported on the device
                Console.WriteLine($"Geolocation is not supported on this device: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                //When no permission
                Console.WriteLine($"Permission to access location was denied: {pEx.Message}");
            }
            catch (Exception ex)
            {
                //When unable to get location
                Console.WriteLine($"Unable to get location: {ex.Message}");
            }
        }

        /// <summary>
        /// Function for loading the map by drawing device and station location on it.
        /// </summary>
        private async void Load()
        {
            var location = new Location(Station.Lat, Station.Lng);

            var mapSpan = new MapSpan(location, 0.01, 00.1);

            stationPin = new Pin
            {
                Label = "Station: " + Station.Namen.Lang,
                Location = new Location(Station.Lat, Station.Lng),
                Type = PinType.Place
            };

            currentLocationPin = new Pin
            {
                Label = "Current location",
                Location = new Location(_location.Latitude, _location.Longitude),
                Type = PinType.Place
            };

            currentLocationCircle = new Circle
            {
                Center = new Location(_location.Latitude, _location.Longitude),
                Radius = Distance.FromMeters(100),
                StrokeColor = Colors.White,
                StrokeWidth = 8,
                FillColor = Colors.Green,
            };

            stationCircle = new Circle
            {
                Center = new Location(Station.Lat, Station.Lng),
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
                    new Location(Station.Lat, Station.Lng),
                    new Location(_location.Latitude, _location.Longitude),
                }
            };

            map.MapElements.Add(line);

            map.MapElements.Add(currentLocationCircle);
            map.MapElements.Add(stationCircle);

            map.Pins.Add(stationPin);
            map.Pins.Add(currentLocationPin);

            map.MoveToRegion(mapSpan);
        }

        /// <summary>
        /// Gets called when device location changed.
        /// Redraws the elements on the map
        /// </summary>
        /// <param name="lat">new y-coordinate of device</param>
        /// <param name="lng">new x-coordinate of device</param>
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
                    new Location(Station.Lat, Station.Lng),
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

        /// <summary>
        /// Sends a notifiction to the notification center of the phone.
        /// </summary>
        /// <param name="whenSeconds">Time from now on, when the notification will arrive.</param>
        /// <param name="title">Title of the notification.</param>
        /// <param name="description">Description of the notification.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts NSStation to a DatabaseStation object
        /// </summary>
        /// <param name="NSStation">The NSStation to be converted</param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets called when user presses the favourite/unfavourite button
        /// </summary>
        [RelayCommand]
        private void Favorites()
        {
            if (!CheckIfInFavourites())
            {
                _database.SaveFavouriteStation(GetStation(Station));
                showNotification(0, "NS Stations", "Added to favorites.");
                ButtonText = "Remove from favourites";
            }
            else
            {
                _database.DeleteFavouriteStationByName(Station.Namen.Lang);
                showNotification(0, "NS Stations", "Removed from favorites.");
                ButtonText = "Add to favourites";
            }
        }

        /// <summary>
        /// Calculates the distance between the device and a station
        /// </summary>
        /// <param name="userLat">y-coordinate of device</param>
        /// <param name="userLong">x-coordinate of device</param>
        /// <param name="stationLat">y-coordinate of station</param>
        /// <param name="stationLong">x-coordinate of station</param>
        /// <returns>The distance between the device and a station</returns>
        private double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
        {
            double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
            return distance;
        }

        /// <summary>
        /// Sets the listener for geolcation changed
        /// </summary>
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

        /// <summary>
        /// Listens for new device locations.
        /// Notifies the users if he is close to a station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            map.MapElements.Clear();
            map.Pins.Clear();
            map.MapElements.Add(stationCircle);
            map.Pins.Add(stationPin);

            updateCurrentLocationElements(e.Location.Latitude, e.Location.Longitude);

            var distance = CalculateDistance(e.Location.Latitude, e.Location.Longitude, Station.Lat, Station.Lng);
            if (distance < 0.15)
            {
                await showNotification(0, "NS Stations", "You are very close to the station.");
            }
        }

        [ObservableProperty]
        NSStation station;
    }
}
