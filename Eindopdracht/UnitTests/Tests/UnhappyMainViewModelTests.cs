﻿namespace UnitTests.Tests.UnhappyTests;

public class UnhappyMainViewModelTests
{
    /// <summary>
    /// This test fails because the VisibleStations list is empty while the stations are being loaded.
    /// So to let this test pass: Assert.Empty(viewModel.VisibleStations)
    /// </summary>
    [Fact]
    public void SearchStations_WhenLoading_ShouldNotPerformSearch()
    {
        IDatabase database = new Database();
        DatabaseService databaseService = new DatabaseService(database);

        INsApi nsApi = new NsApi();
        NsApiService nsApiService = new NsApiService(nsApi);

        var viewModel = new MainViewModel(databaseService, nsApiService);

        viewModel.IsLoading = true;
        viewModel.SearchQuery = "Station";
        viewModel.VisibleStations = new List<NSStation>();

        var allStations = new List<NSStation>
        {
            new NSStation
            {
                Id = 1,
                Namen = new NSStationNamen { Lang = "Station Alpha" },
                Distance = 0.1f,
                Lat = 0.1f,
                Lng = 0.1f,
                Naam = "Test station",
                StationType = "test station",
                HeeftFaciliteiten = true,
                HeeftReisassistentie = true,
                Land = "NL"
            },
            new NSStation
            {
                Id = 2,
                Namen = new NSStationNamen { Lang = "Station Alpha" },
                Distance = 0.1f,
                Lat = 0.1f,
                Lng = 0.1f,
                Naam = "Test station",
                StationType = "test station",
                HeeftFaciliteiten = true,
                HeeftReisassistentie = true,
                Land = "NL"
            }
        };

        viewModel.AllStations = allStations;

        viewModel.SearchStations();

        Assert.NotEmpty(viewModel.VisibleStations);
    }

    /// <summary>
    /// This test fails because an invalid value is set for AllStations to simulate failure.
    /// So to let this test pass: Assert.Null(viewModel.VisibleStations)
    /// </summary>
    [Fact]
    public void SetStations_WhenOptionIsAll_ShouldFail()
    {
        IDatabase database = new Database();
        DatabaseService databaseService = new DatabaseService(database);

        INsApi nsApi = new NsApi();
        NsApiService nsApiService = new NsApiService(nsApi);

        var viewModel = new MainViewModel(databaseService, nsApiService);

        var allStations = new List<NSStation>
        {
            new NSStation { Id = 1, Naam = "Station A", Distance = 10.0f },
            new NSStation { Id = 2, Naam = "Station B", Distance = 20.0f }
        };

        viewModel.AllStations = null;

        viewModel.SetStations(0);
        
        Assert.NotNull(viewModel.VisibleStations);
    }

    /// <summary>
    /// This test fails because an invalid value is set for FavouriteStations to simulate failure.
    /// So to let this test pass: Assert.Null(viewModel.VisibleStations)
    /// </summary>
    [Fact]
    public void SetStations_WhenOptionIsFavourites_ShouldFail()
    {
        IDatabase database = new Database();
        DatabaseService databaseService = new DatabaseService(database);

        INsApi nsApi = new NsApi();
        NsApiService nsApiService = new NsApiService(nsApi);

        var viewModel = new MainViewModel(databaseService, nsApiService);

        var favouriteStations = new List<NSStation>
            {
                new NSStation { Id = 1, Naam = "Fav Station A", Distance = 5.0f },
                new NSStation { Id = 2, Naam = "Fav Station B", Distance = 15.0f }
            };

        viewModel.FavouriteStations = null;

        viewModel.SetStations(2);

        Assert.Null(viewModel.VisibleStations);
    }
}