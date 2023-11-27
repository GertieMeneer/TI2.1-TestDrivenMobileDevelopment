using Eindopdracht.NSData;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
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

            // Add a pin for the station
            var stationPin = new Pin
            {
                Label = "Station: " + stationToFind.Name,
                Location = new Location(stationToFind.Lat, stationToFind.Lng),
                Type = PinType.Place
            };

            double locationLat = Convert.ToDouble(Preferences.Get("locationLat", "error 404"));
            double locationLng = Convert.ToDouble(Preferences.Get("locationLng", "error 404"));

            // Add a pin for the station
            var currentLocationPin = new Pin
            {
                Label = "Current location",
                Location = new Location(locationLat, locationLng),
                Type = PinType.Place
            };

            map.Pins.Add(stationPin);
            map.Pins.Add(currentLocationPin);

            map.MoveToRegion(mapSpan);

            showNotification(5, "Eindopdracht", "Map loaded succesfully!");
        }
    }

    private async void showNotification(int whenSeconds, string title, string description)
    {
        await LocalNotificationCenter.Current.RequestNotificationPermission();

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