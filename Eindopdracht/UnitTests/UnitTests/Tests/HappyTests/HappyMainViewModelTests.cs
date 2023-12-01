using Eindopdracht.NSData;
using Eindopdracht.ViewModels;

namespace UnitTests.Tests.HappyTests;

public class HappyMainViewModelTests
{
    [Fact]
    public void SetStations_WhenOptionIsAll_ShouldSetVisibleStationsToAllStations()
    {
        var viewModel = new MainViewModel();
        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Naam = "Station A", Distance = 10.0f },
            new NSStation { Id = 2, Naam = "Station B", Distance = 20.0f }
        };

        viewModel.AllStations = allStations;

        viewModel.SetStations(0);

        Assert.Equal(allStations, viewModel.VisibleStations);
    }

    [Fact]
    public void SearchStations_WhenNotLoading_ShouldFilterStationsBySearchQuery()
    {
        var viewModel = new MainViewModel();
        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Namen = new NSStationNamen { Lang = "Station Alpha" } },
            new NSStation { Id = 2, Namen = new NSStationNamen { Lang = "Station Beta" } }
        };

        viewModel.AllStations = allStations;
        viewModel.IsLoading = false;
        viewModel.SearchQuery = "Alpha";

        viewModel.SearchStations();

        Assert.Collection(viewModel.VisibleStations,
            station => Assert.Equal("Station Alpha", station.Namen.Lang));
    }
    
    [Fact]
    public void SetStations_WhenOptionIsFavourites_ShouldSetVisibleStationsToFavouriteStations()
    {
        var viewModel = new MainViewModel();
        
        var favouriteStations = new List<NSStation>
        {
            new NSStation { Id = 1, Naam = "Fav Station A", Distance = 5.0f },
            new NSStation { Id = 2, Naam = "Fav Station B", Distance = 15.0f }
        };
        viewModel.VisibleStations = new List<NSStation>();

        viewModel.FavouriteStations = favouriteStations;

        viewModel.SetStations(2);

        Assert.Equal(viewModel.FavouriteStations, viewModel.VisibleStations);
    }
}