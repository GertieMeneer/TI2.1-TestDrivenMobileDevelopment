namespace UnitTests.Tests.UnhappyTests
{
    public class UnhappyMainPageTests
    {
        /// <summary>
        /// This test ensures that GetAllNSStationsTest fails when there is a failure scenario during the API call.
        /// </summary>
        [Fact]
        public static async Task GetAllNSStationsTest_ShouldFail()
        {
            IDatabase database = new Database();
            var viewModel = new MainViewModel(database);

            double latitude = 52.3702;
            double longitude = 4.8952;

            var stations = await viewModel.GetAllNSStations(latitude, longitude);

            Assert.Null(stations);
        }

        /// <summary>
        /// This test ensures that CalculateDistanceTest fails when there is a scenario where calculation fails.
        /// </summary>
        [Fact]
        public void CalculateDistanceTest_ShouldFail()
        {
            IDatabase database = new Database();
            var viewModel = new MainViewModel(database);

            double userLat = 52.3702;
            double userLong = 4.8952;
            double stationLat = 51.9225;
            double stationLong = 4.47917;

            double distance = viewModel.CalculateDistance(userLat, userLong, stationLat, stationLong);

            Assert.True(double.IsNaN(distance));
        }
    }
}