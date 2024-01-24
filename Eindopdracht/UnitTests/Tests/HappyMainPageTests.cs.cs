namespace UnitTests.Tests.HappyTests;

public class HappyMainPageTests
{
    /// <summary>
    /// This test ensures that GetAllNSStations returns a non-null and non-empty list of stations.
    /// </summary>
    [Fact]
    public static async Task GetAllNSStationsTest()
    {
        IDatabase database = new Database();
        var viewModel = new MainViewModel(database);

        // Arrange
        double latitude = 52.3702;
        double longitude = 4.8952;

        // Act
        var stations = await viewModel.GetAllNSStations(latitude, longitude);

        // Assert
        Assert.NotNull(stations);
        Assert.NotEmpty(stations);
    }

    /// <summary>
    /// This test ensures that CalculateDistance returns a positive distance value.
    /// </summary>
    [Fact]
    public void CalculateDistanceTest()
    {
        IDatabase database = new Database();
        var viewModel = new MainViewModel(database);

        double userLat = 52.3702;
        double userLong = 4.8952;
        double stationLat = 51.9225;
        double stationLong = 4.47917;

        double distance = viewModel.CalculateDistance(userLat, userLong, stationLat, stationLong);

        Assert.True(distance > 0);
    }
}