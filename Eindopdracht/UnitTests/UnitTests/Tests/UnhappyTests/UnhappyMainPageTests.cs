using Eindopdracht;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Arrange
            double latitude = 52.3702;
            double longitude = 4.8952;

            // Simulate a failure scenario during API call
            // Act
            var stations = await MainPage.GetAllNSStations(latitude, longitude);

            // Assert
            Assert.Null(stations);
        }

        /// <summary>
        /// This test ensures that CalculateDistanceTest fails when there is a scenario where calculation fails.
        /// </summary>
        [Fact]
        public void CalculateDistanceTest_ShouldFail()
        {
            // Arrange
            double userLat = 52.3702;
            double userLong = 4.8952;
            double stationLat = 51.9225;
            double stationLong = 4.47917;

            // Simulate a scenario where calculation fails
            // Act
            double distance = MainPage.CalculateDistance(userLat, userLong, stationLat, stationLong);

            // Assert
            Assert.True(double.IsNaN(distance));
        }
    }
}
