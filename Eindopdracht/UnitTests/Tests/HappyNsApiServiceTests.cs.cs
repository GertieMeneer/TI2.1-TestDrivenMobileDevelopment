namespace UnitTests.Tests.HappyTests;

public class HappyNsApiServiceTests
{
    /// <summary>
    /// This test ensures that GetAllNSStations returns a non-null and non-empty list of stations.
    /// </summary>
    [Fact]
    public static async Task GetAllNSStationsTest()
    {
        INsApi nsApi = new NsApi();
        NsApiService nsApiService = new NsApiService(nsApi);

        double latitude = 52.3702;
        double longitude = 4.8952;

        var stations = await nsApiService.getAllStations(latitude, longitude);

        //assert
        Assert.NotNull(stations);
        Assert.NotEmpty(stations);
    }
}