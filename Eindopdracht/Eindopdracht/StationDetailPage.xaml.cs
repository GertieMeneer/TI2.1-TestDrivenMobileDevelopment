using Eindopdracht.NSData;
using Eindopdracht.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Plugin.LocalNotification;

namespace Eindopdracht;

public partial class StationDetailPage
{
    private NSStation _station;
    private bool _isFavourite;

    public StationDetailPage(NSStation station)
    {
        _station = station;
        InitializeComponent();
        Title = $"{station.Naam} - Details";
        BindingContext = new StationDetailViewModel(station);
        Load();
        CheckFavourite();
    }

    private void CheckFavourite()
    {
        //compare with sqllite database: if in favourites database: button text changes

        // foreach(NSStation station in SQLLITEDATABASE)
        // {
        //     if (station == _station)
        //     {
        //         FavoritesButton.Text = "Remove from favourites";
        //          _isFavourite == true;
        //     }
        //     else
        //     {
        //         FavoritesButton.Text = "Add to favourites";
        //          _isFavourite == false;
        //     }
        // }
    }

    private async void Load()
    {
        var location = new Location(_station.Lat, _station.Lng);

        var mapSpan = new MapSpan(location, 0.01, 00.1);

        var stationPin = new Pin
        {
            Label = "Station: " + _station.Naam,
            Location = new Location(_station.Lat, _station.Lng),
            Type = PinType.Place
        };

        double locationLat = Convert.ToDouble(Preferences.Get("locationLat", "error 404"));
        double locationLng = Convert.ToDouble(Preferences.Get("locationLng", "error 404"));

        var currentLocationPin = new Pin
        {
            Label = "Current location",
            Location = new Location(locationLat, locationLng),
            Type = PinType.Place
        };

        map.Pins.Add(stationPin);
        map.Pins.Add(currentLocationPin);

        map.MoveToRegion(mapSpan);

        await showNotification(5, "Eindopdracht", "Map loaded succesfully!");
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

    private void FavoritesButton_OnClicked(object? sender, EventArgs e)
    {
        if (!_isFavourite)
        {
            //add _station to favourite database
            //miss toast notification of echte notification als station removed of added is aan database nunununutnut
        }
        else
        {
            //remove _station from favourite database
        }
    }
}