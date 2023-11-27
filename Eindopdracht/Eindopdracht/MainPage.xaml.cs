using Eindopdracht.NSData;
using Eindopdracht.ViewModels;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace Eindopdracht
{
    public partial class MainPage : ContentPage
    {
        static string NSAPIKey = "12ef36ad08a1435597ae44c554d62ef8";
        // const string GoogleMapsAPIKey = "AIzaSyBXG_XrA3JRTL58osjxd0DbqH563e2t84o";
        private static HttpClient httpClient;
        private static Location location;
        private static MainViewModel _viewModel;
        public static ListView stations;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.BindingContext = _viewModel;
            searchBar.BindingContext = _viewModel;
            stations = stationListView;


            httpClient = new HttpClient();
            TaskFindNearestStations();

            //var location = new Location(36, -122);
            //var mapSpan = new MapSpan(location, 0.01, 0.01);

            //GetCurrentLocationAndSetIt();

            //showNotification();

            // string infoToSave = "Hallo ja dit is dus info die opgelagen moet worden weet je wel!";
            //
            // Preferences.Set("SaveInfo", infoToSave);
            //
            // var savedPreference = Preferences.Get("SaveInfo", "error 404");
            //
            // Toast.Make(savedPreference, ToastDuration.Long, 30).Show();

        }

        public static async Task TaskFindNearestStations()
        {
            await GetCurrentLocationAndSetIt();

            if (location != null)
            {
                double userLatitude = location.Latitude;
                double userLongitude = location.Longitude;

                List<NSStation> allStations = await GetNearestNSStations(userLatitude, userLongitude);

                // store all the stations in a preference
                string allStationsJson = JsonConvert.SerializeObject(allStations);
                Preferences.Set("allStationsInJSON", allStationsJson);

                allStations.Sort((s1, s2) => s1.Distance.CompareTo(s2.Distance));
                List<NSStation> nearestStations = allStations.Take(10).ToList();

                _viewModel.AllStations = allStations;
                _viewModel.NearestStations = nearestStations;
                _viewModel.SetStations();
            }
        }

        public static async Task GetCurrentLocationAndSetIt()
        {
            try
            {
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                location = await Geolocation.GetLocationAsync(request);
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

        public static async Task<List<NSStation>> GetNearestNSStations(double latitude, double longitude)
        {

            List<NSStation> stations = new List<NSStation>();

                string nsStationsUrl = $"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/stations?latitude={latitude}&longitude={longitude}";
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", NSAPIKey);

                var nsResponse = await httpClient.GetStringAsync(nsStationsUrl);

                var nsStationsPayload = JsonConvert.DeserializeObject<NSStationPayload>(nsResponse);

                foreach (var station in nsStationsPayload.Payload)
                {
                    double stationLat = station.Lat;
                    double stationLng = station.Lng;

                    double distance = CalculateDistance(latitude, longitude, stationLat, stationLng);

                    // Voeg station en afstand toe aan de lijst
                    stations.Add(new NSStation
                    {
                        Name = station.Namen.Lang,
                        Distance = distance,
                        Lat = stationLat,
                        Lng = stationLng
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
                Navigation.PushAsync(new StationDetailPage(selectedStation.Name));
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
