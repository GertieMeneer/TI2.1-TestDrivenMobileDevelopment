using Microsoft.Maui.Maps;

namespace Eindopdracht
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //var location = new Location(36, -122);
            //var mapSpan = new MapSpan(location, 0.01, 0.01);

            GetCurrentLocationAndSetIt();
        }
        
        public async Task GetCurrentLocationAndSetIt()
        {   
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(60));

            Location location = await Geolocation.Default.GetLocationAsync(request);

            var mapSpan = new MapSpan(location, 0.01, 0.01);

            map.MoveToRegion(mapSpan);
        }
    }

}
