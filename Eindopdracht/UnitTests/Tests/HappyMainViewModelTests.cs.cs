namespace UnitTests.Tests.HappyTests;

public class HappyMainViewModelTests
{
    /// <summary>
    /// This test ensures that when the option is set to All, the VisibleStations are correctly set to AllStations.
    /// </summary>
    [Fact]
    public void SetStations_WhenOptionIsAll_ShouldSetVisibleStationsToAllStations()
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

        viewModel.AllStations = allStations;

        viewModel.SetStations(0);

        Assert.Equal(allStations, viewModel.VisibleStations);
    }

    /// <summary>
    /// This test ensures that when not loading and a search query is provided, the VisibleStations are correctly filtered.
    /// </summary>
    [Fact]
    public void SearchStations_WhenNotLoading_ShouldFilterStationsBySearchQuery()
    {
        IDatabase database = new Database();
        DatabaseService databaseService = new DatabaseService(database);

        INsApi nsApi = new NsApi();
        NsApiService nsApiService = new NsApiService(nsApi);

        var viewModel = new MainViewModel(databaseService, nsApiService);

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

    /// <summary>
    /// This test ensures that when the option is set to Favourites, the VisibleStations are correctly set to FavouriteStations.
    /// </summary>
    [Fact]
    public void SetStations_WhenOptionIsFavourites_ShouldSetVisibleStationsToFavouriteStations()
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
        viewModel.VisibleStations = new List<NSStation>();

        viewModel.FavouriteStations = favouriteStations;

        viewModel.SetStations(2);

        Assert.Equal(viewModel.FavouriteStations, viewModel.VisibleStations);
    }
}
