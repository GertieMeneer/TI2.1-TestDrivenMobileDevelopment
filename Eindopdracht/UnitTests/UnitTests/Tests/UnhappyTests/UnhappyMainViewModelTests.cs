using Eindopdracht.NSData;
using Eindopdracht.ViewModels;

namespace UnitTests.Tests.UnhappyTests;

public class UnhappyMainViewModelTests
{
    /// <summary>
    /// This test fails because the VisibleStations list is empty while the stations are being loaded.
    /// So to let this test pass: Assert.Empty(viewModel.VisibleStations)
    /// </summary>
    [Fact]
    public void SearchStations_WhenLoading_ShouldNotPerformSearch()
    {
        var viewModel = new MainViewModel();
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

        Assert.NotEmpty(viewModel.VisibleStations); // Ensure that no search was performed while loading
    }
}