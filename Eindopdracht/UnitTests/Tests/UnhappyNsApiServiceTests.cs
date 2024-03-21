namespace UnitTests.Tests.UnhappyTests
{
    public class UnhappyNsApiServiceTests
    {
        /// <summary>
        /// This test ensures that GetAllNSStationsTest fails when there is a failure scenario during the API call.
        /// </summary>
        [Fact]
        public static async Task GetAllNSStationsTest_ShouldFail()
        {
            INsApi nsApi = new NsApi();
            NsApiService nsApiService = new NsApiService(nsApi);

            double latitude = 52.3702;
            double longitude = 4.8952;

            var stations = await nsApiService.getAllStations(latitude, longitude);

            Assert.Null(stations);
        }
    }
}