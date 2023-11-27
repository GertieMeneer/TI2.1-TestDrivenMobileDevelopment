using Eindopdracht.NSData;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace Eindopdracht;

public partial class StationDetailPage : ContentPage
{
	public StationDetailPage(string stationName)
	{
		InitializeComponent();

        var savedPreferenceOfAllStationJSON = Preferences.Get("allStationsInJSON", "error 404");

        List<NSStation> allStations = JsonConvert.DeserializeObject<List<NSStation>>(savedPreferenceOfAllStationJSON);

		NSStation stationToFind = null;

        foreach (NSStation station in allStations)
		{
			if (station.Name == stationName)
			{
                stationToFind = station;
                break;
			}
		}

        if (stationToFind != null) 
        {
            var location = new Location(stationToFind.Lat, stationToFind.Lng);
            var mapSpan = new MapSpan(location, 0.01, 0.01);
            map.MoveToRegion(mapSpan);
        }
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