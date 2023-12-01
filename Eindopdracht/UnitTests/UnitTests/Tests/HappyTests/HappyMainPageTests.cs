using Eindopdracht;

namespace UnitTests.Tests.HappyTests;

public class HappyMainPageTests
{
    
    [Fact]
    public static async Task GetAllNSStationsTest()
    {
        // Arrange
        double latitude = 52.3702;
        double longitude = 4.8952;

        // Act
        var stations = await MainPage.GetAllNSStations(latitude, longitude);

        // Assert
        Assert.NotNull(stations);
        Assert.NotEmpty(stations);
    }
    
    [Fact]
    public void CalculateDistanceTest()
    {
        // Arrange
        double userLat = 52.3702;
        double userLong = 4.8952;
        double stationLat = 51.9225;
        double stationLong = 4.47917;

        // Act
        double distance = MainPage.CalculateDistance(userLat, userLong, stationLat, stationLong);

        // Assert
        Assert.True(distance > 0);
    }


}