using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Maps;
using Plugin.LocalNotification;

namespace Eindopdracht
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //var location = new Location(36, -122);
            //var mapSpan = new MapSpan(location, 0.01, 0.01);

            //GetCurrentLocationAndSetIt();

            //showNotification();

            string infoToSave = "Hallo ja dit is dus info die opgelagen moet worden weet je wel!";
          
            Preferences.Set("SaveInfo", infoToSave);
     
            var savedPreference = Preferences.Get("SaveInfo", "error 404");

            Toast.Make(savedPreference, ToastDuration.Long, 30).Show();

        }
        
        public async Task GetCurrentLocationAndSetIt()
        {   
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(60));

            Location location = await Geolocation.Default.GetLocationAsync(request);

            var mapSpan = new MapSpan(location, 0.01, 0.01);

            map.MoveToRegion(mapSpan);
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
