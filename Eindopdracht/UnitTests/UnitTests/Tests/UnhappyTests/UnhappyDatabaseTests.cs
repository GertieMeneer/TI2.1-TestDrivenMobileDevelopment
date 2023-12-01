using Eindopdracht.NSData;

namespace UnitTests.Tests.UnhappyTests
{
    public class UnhappyDatabaseTests
    {
        /// <summary>
        /// This test ensures that SaveFavouriteStation fails when attempting to save a favorite station with an invalid Distance value.
        /// </summary>
        [Fact]
        public void SaveFavouriteStation_ShouldFail()
        {
            Database database = new Database();
            var station = new DatabaseStation
            {
                // Invalid Distance value to make the test fail
                Distance = -1,
                Lat = 0,
                Lng = 0,
                Naam = "Save Favourite Station Database Unhappy Test",
                StationType = "TEST_STATION",
                HeeftFaciliteiten = true,
                HeeftReisassistentie = true,
                Land = "NL"
            };

            Assert.Throws<Exception>(() => database.SaveFavouriteStation(station));
        }

        /// <summary>
        /// This test ensures that DeleteFavouriteStationByName fails when attempting to delete a nonexistent favorite station by name.
        /// Assuming there is no station named "Nonexistent Station" in the database.
        /// </summary>
        [Fact]
        public void DeleteFavouriteStationByName_ShouldFail()
        {
            Database database = new Database();
            // Assuming there is no station named "Nonexistent Station" in the database
            string stationNameToDelete = "Nonexistent Station";

            Assert.Throws<Exception>(() => database.DeleteFavouriteStationByName(stationNameToDelete));
        }
    }
}