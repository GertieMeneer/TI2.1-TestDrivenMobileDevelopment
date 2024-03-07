using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using CommunityToolkit.Mvvm.Input;

using Eindopdracht.Interfaces;
using Eindopdracht.NSData;

using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace Eindopdracht.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private readonly string _nsapiKey = "12ef36ad08a1435597ae44c554d62ef8";
        private HttpClient _httpClient;

        private List<NSStation> _visibleStations;   //stations that are currently visible in ui
        private List<NSStation> _allStations;       //all stations
        private List<NSStation> _nearestStations;   //10 nearest stations
        private List<NSStation> _favouriteStations; //favourited stations

        private string _searchQuery;
        private Location _location;
        private int _selectedSortIndex = 0;
        private bool _isRefreshing = false;
        private bool _isLoading;

        private IDatabase _database;

        IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

        public MainViewModel(IDatabase database)
        {
            _httpClient = new HttpClient();

            TaskGetStations();

            SelectedSortIndex = 0;
            _database = database;
        }

        //public Location Location
        //{
        //    get => _location;
        //    set => _location = value;
        //}

        public int SelectedSortIndex
        {
            get => _selectedSortIndex;
            set
            {
                _selectedSortIndex = value;
                SetStations(_selectedSortIndex);
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
            }
        }

        public List<NSStation> VisibleStations
        {
            get => _visibleStations;
            set
            {
                _visibleStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> NearestStations
        {
            get => _nearestStations;
            set
            {
                _nearestStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> AllStations
        {
            get => _allStations;
            set
            {
                _allStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> FavouriteStations
        {
            get => _favouriteStations;
            set
            {
                _favouriteStations = value;
            }
        }

        /// <summary>
        /// Sets the visible stations in the app based on the users selected input
        /// </summary>
        /// <param name="option">The user selected option</param>
        /// <exception cref="Exception"></exception>
        public void SetStations(int option)
        {
            switch (option)
            {
                case 0:
                    VisibleStations = AllStations;
                    break;
                case 1:
                    VisibleStations = NearestStations;
                    break;
                case 2:
                    FavouriteStations = new List<NSStation>();
                    foreach (DatabaseStation station in _database.GetFavouriteStations())
                    {
                        NSStation NSStation = new NSStation()
                        {
                            Id = station.Id,
                            Distance = station.Distance,
                            Lat = station.Lat,
                            Lng = station.Lng,
                            Naam = station.Naam,
                            StationType = station.StationType,
                            HeeftFaciliteiten = station.HeeftFaciliteiten,
                            HeeftReisassistentie = station.HeeftReisassistentie,
                            Land = station.Land,
                            Namen = new NSStationNamen()
                            {
                                Lang = station.Naam,
                                Middel = "",
                                Kort = ""
                            }
                        };
                        FavouriteStations.Add(NSStation);
                    }
                    VisibleStations = FavouriteStations;
                    break;
                default:
                    VisibleStations = AllStations; break;
            }
        }

        //RelayCommands. Passes execution from the view to the viewmodel,
        //without directly connecting them together.

        /// <summary>
        /// RelayCommands for opening the station detail page
        /// </summary>
        /// <param name="nsStation">Which station to open</param>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToStationDetailPage(NSStation nsStation)
        {
            if (nsStation == null)
                return;

            Debug.WriteLine(nameof(NSStation));

            await Shell.Current.GoToAsync(nameof(StationDetailPage), true, new Dictionary<string, object>
            {
                {"s", nsStation }
            });
        }


        /// <summary>
        /// Searches in the list of all stations.
        /// </summary>
        [RelayCommand]
        public void SearchStations()
        {
            if (!IsLoading)
            {
                VisibleStations = AllStations.FindAll(station => station.Namen.Lang.ToLower().Contains(SearchQuery.ToLower()));
            }
            else
            {
                showNotification(0, "ERROR", "Cannot search while the app is loading");
            }
        }

        /// <summary>
        /// Executes when pull to refresh
        /// </summary>
        [RelayCommand]
        private void Refresh()
        {
            var option = SelectedSortIndex;

            IsRefreshing = true;

            TaskGetStations();

            SetStations(option);

            IsRefreshing = false;
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
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        /// <summary>
        /// Event handler for changing properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handles all stations from NS api.
        /// Sorts all stations based on distance, then gets the 10 closest.
        /// Sorts all stations based on name.
        /// Then sets the stations in the app. Default is 10 closest.
        /// </summary>
        /// <returns>Task</returns>
        public async Task TaskGetStations()
        {
            IsLoading = true;        //show loading indicator

            if (!CheckInternet())
            {
                showNotification(0, "Error", "Make sure you are connected to the internet");
                IsLoading = false; return;
            }

            await GetCurrentLocationAndSetIt();     //get location

            if (_location != null)
            {
                double userLatitude = _location.Latitude;
                double userLongitude = _location.Longitude;

                List<NSStation> allStations = await GetAllNSStations(userLatitude, userLongitude);      //get all stations from ns api
                allStations.Sort((s1, s2) => s1.Distance.CompareTo(s2.Distance));       //sort based on distance
                List<NSStation> nearestStations = allStations.Take(10).ToList();        //get 10 closest stations to show as default in app
                allStations.Sort((s1, s2) => string.Compare(s1.Namen.Lang, s2.Namen.Lang));     //sort based on name

                //set all the lists in the viewmodel
                AllStations = allStations;
                NearestStations = nearestStations;

                SetStations(_selectedSortIndex);  //set the default list for the listview when starting app (1 = top 10 nearest)
            }

            IsLoading = false;       //hide loading indicator
        }

        /// <summary>
        /// Gets the current location of the device and sets it.
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
        /// Sends api request to NS api in order to get all the stations from the api.
        /// Then calculates the distance from device to station for each received station.
        /// </summary>
        /// <param name="latitude">y-coordinate of the device</param>
        /// <param name="longitude">x-coordinate of the device</param>
        /// <returns>List with all the NS Stations</returns>
        public async Task<List<NSStation>> GetAllNSStations(double latitude, double longitude)
        {
            List<NSStation> stations = new List<NSStation>();

            string nsStationsUrl = $"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/stations?latitude={latitude}&longitude={longitude}";
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _nsapiKey);

            var nsResponse = await _httpClient.GetStringAsync(nsStationsUrl);

            var nsStationsPayload = JsonConvert.DeserializeObject<NSStationPayload>(nsResponse);

            foreach (NSStation station in nsStationsPayload.Payload)
            {
                double stationLat = station.Lat;
                double stationLng = station.Lng;

                double distance = CalculateDistance(latitude, longitude, stationLat, stationLng);

                stations.Add(new NSStation
                {
                    Namen = station.Namen,
                    Distance = distance,
                    Lat = station.Lat,
                    Lng = station.Lng,
                    StationType = station.StationType,
                    HeeftFaciliteiten = station.HeeftFaciliteiten,
                    HeeftReisassistentie = station.HeeftReisassistentie,
                    Land = station.Land,
                });
            }
            return stations;
        }

        /// <summary>
        /// Calculates the distance between the device and a station
        /// </summary>
        /// <param name="userLat">y-coordinate of device</param>
        /// <param name="userLong">x-coordinate of device</param>
        /// <param name="stationLat">y-coordinate of station</param>
        /// <param name="stationLong">x-coordinate of station</param>
        /// <returns>The distance between the device and a station</returns>
        public double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
        {
            double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
            return distance;
        }

        /// <summary>
        /// Checks if user is connected to some sort of ineternet network.
        /// Based on the return value, the app will notify the user.
        /// </summary>
        /// <returns>True if connected, false if not connected</returns>
        public bool CheckInternet()
        {
            if (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Cellular) || profiles.Contains(ConnectionProfile.Ethernet))
            {
                return true;
            }
            return false;
        }
    }
}