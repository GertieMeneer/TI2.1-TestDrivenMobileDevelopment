using Eindopdracht.NSData;
using Eindopdracht.ViewModels;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace Eindopdracht
{
    public partial class MainPage
    {
        private static string _nsapiKey = "12ef36ad08a1435597ae44c554d62ef8";
        private static HttpClient? _httpClient;
        private static Location? _location;
        private static MainViewModel? _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            BindingContext = _viewModel;
            searchBar.BindingContext = _viewModel;
            ListSorter.SelectedIndex = 1;

            _httpClient = new HttpClient();
            _ = TaskGetStations();
        }

        public static async Task TaskGetStations()
        {
            _viewModel.IsLoading = true;
            await GetCurrentLocationAndSetIt();

            if (_location != null)
            {
                double userLatitude = _location.Latitude;
                double userLongitude = _location.Longitude;

                List<NSStation> allStations = await GetAllNSStations(userLatitude, userLongitude);      //fetch all stations from ns api
                allStations.Sort((s1, s2) => string.Compare(s1.Naam, s2.Naam)); //sort based on name
                List<NSStation> nearestStations = allStations.Take(10).ToList();        //get 10 closest stations to show as default in app
                nearestStations.Sort((s1, s2) => s1.Distance.CompareTo(s2.Distance));   //sort 10 nearest based on distance

                // List<NSStation> favouriteStations = en dan hier je code om list uit datatbase te halen

                _viewModel.AllStations = allStations;
                _viewModel.NearestStations = nearestStations;
                // _viewModel.FavouriteStations = favouriteStations;        uncomment deze als lijst uit database werkt :D

                _viewModel.SetStations(1);      //deze altijd op 1 houden, top 10 nearest stations is altijd default
            }

            _viewModel.IsLoading = false;
        }

        public static async Task GetCurrentLocationAndSetIt()
        {
            try
            {
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                _location = await Geolocation.GetLocationAsync(request);
                _viewModel.Location = _location;
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

        public static async Task<List<NSStation>> GetAllNSStations(double latitude, double longitude)
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

                    // Voeg station toe aan de lijst
                    stations.Add(new NSStation
                    {
                        Naam = station.Namen.Lang,
                        Distance = distance,
                        Lat = station.Lat,
                        Lng = station.Lng,
                        StationType = station.StationType,
                        HeeftFaciliteiten = station.HeeftFaciliteiten,
                        HeeftReisassistentie = station.HeeftReisassistentie,
                        Land = station.Land
                    });
                }
            return stations;
        }

        private static double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
        {
            double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
            return distance;
        }

        private void OnStationTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is NSStation selectedStation)
            {                
                Navigation.PushAsync(new StationDetailPage(selectedStation, _location));
            }

            ((ListView)sender).SelectedItem = null;
        }

        private void ListSorter_OnSelectedIndexChanged(object? sender, EventArgs e)
        {
            var picker = sender as Picker;
            int selectedOption = picker.SelectedIndex;

            _viewModel.SetStations(selectedOption);
        }
    }
}
