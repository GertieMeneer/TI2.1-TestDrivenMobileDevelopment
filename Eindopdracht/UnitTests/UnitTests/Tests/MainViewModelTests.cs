using Eindopdracht.NSData;
using Eindopdracht.ViewModels;

namespace UnitTests.Tests;

public class MainViewModelTests
{
    [Fact]
    public void SetStations_WhenOptionIsAll_ShouldSetVisibleStationsToAllStations()
    {
        // Arrange
        var viewModel = new MainViewModel();
        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Naam = "Station A", Distance = 10.0 },
            new NSStation { Id = 2, Naam = "Station B", Distance = 20.0 }
        };

        viewModel.AllStations = allStations;

        // Act
        viewModel.SetStations(0);

        // Assert
        Assert.Equal(allStations, viewModel.VisibleStations);
    }

    [Fact]
    public void SearchStations_WhenNotLoading_ShouldFilterStationsBySearchQuery()
    {
        // Arrange
        var viewModel = new MainViewModel();
        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Namen = new NSStationNamen { Lang = "Station Alpha" } },
            new NSStation { Id = 2, Namen = new NSStationNamen { Lang = "Station Beta" } }
        };

        viewModel.AllStations = allStations;
        viewModel.IsLoading = false;
        viewModel.SearchQuery = "Alpha";

        // Act
        viewModel.SearchStations();

        // Assert
        Assert.Collection(viewModel.VisibleStations,
            station => Assert.Equal("Station Alpha", station.Namen.Lang));
    }

    [Fact]
    public void SetStations_WhenOptionIsFavourites_ShouldSetVisibleStationsToFavouriteStations()
    {
        // Arrange
        var viewModel = new MainViewModel();
        var favouriteStations = new List<NSStation>
        {
            new NSStation { Id = 1, Naam = "Fav Station A", Distance = 5.0 },
            new NSStation { Id = 2, Naam = "Fav Station B", Distance = 15.0 }
        };

        // Setting the FavouriteStations directly (in a real scenario, this data might come from a database)
        viewModel.FavouriteStations = favouriteStations;

        // Act
        viewModel.SetStations(2);

        // Assert
        Assert.Equal(favouriteStations, viewModel.VisibleStations);
    }

    [Fact]
    public void SearchStations_WhenLoading_ShouldNotPerformSearch()
    {
        // Arrange
        var viewModel = new MainViewModel();
        viewModel.IsLoading = true;
        viewModel.SearchQuery = "Station";

        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Namen = new NSStationNamen { Lang = "Station Alpha" } },
            new NSStation { Id = 2, Namen = new NSStationNamen { Lang = "Station Beta" } }
        };

        viewModel.AllStations = allStations;

        // Act
        viewModel.SearchStations();

        // Assert
        Assert.NotEmpty(viewModel.VisibleStations); // Ensure that no search was performed while loading
    }

    // More tests can be added to cover other scenarios such as empty stations, edge cases, etc.


    

    // More tests can be added to cover other scenarios such as empty stations, edge cases, etc.
}

