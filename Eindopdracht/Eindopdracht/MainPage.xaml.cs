using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace Eindopdracht
{
    public partial class MainPage : ContentPage
    {
        const string NSAPIKey = "12ef36ad08a1435597ae44c554d62ef8";
        const string GoogleMapsAPIKey = "AIzaSyBXG_XrA3JRTL58osjxd0DbqH563e2t84o";
        HttpClient httpClient;
        private Location location;
        public MainPage()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            FindNearestStations();

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

        private async void FindNearestStations()
        {
            await GetCurrentLocationAndSetIt();

            if (location != null)
            {
                double userLatitude = location.Latitude;
                double userLongitude = location.Longitude;

                List<NSStation> nearestStations = await GetNearestNSStations(userLatitude, userLongitude);
                nearestStations.Sort((s1, s2) => s1.Distance.CompareTo(s2.Distance));
                List<NSStation> firstTenStations = nearestStations.Take(10).ToList();


                stationListView.ItemsSource = firstTenStations;
            }
            else
            {
                showNotification();
            }
        }

        public async Task GetCurrentLocationAndSetIt()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                location = await Geolocation.GetLocationAsync(request);

                // if (location != null)
                // {
                //     var mapSpan = new MapSpan(location, 0.01, 0.01);
                //     map.MoveToRegion(mapSpan);
                // }
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

        private async Task<List<NSStation>> GetNearestNSStations(double latitude, double longitude)
        {

            List<NSStation> stations = new List<NSStation>();

            try
            {
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
                        Distance = distance
                    });
                }
            }
            catch (Exception ex)
            {
                // Behandel eventuele fouten bij het ophalen van gegevens
                Console.WriteLine($"Fout bij het ophalen van stations: {ex.Message}");
            }

            return stations;
        }

        private double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
        {
            double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
            return distance;
        }

        private async void showNotification()
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();

            var request = new NotificationRequest
            {
                NotificationId = 1000,
                Title = "App",
                Subtitle = "Sick you started the app",
                Description = "Description",
                BadgeNumber = 50,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.FromSeconds(60),
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }
    }
}
