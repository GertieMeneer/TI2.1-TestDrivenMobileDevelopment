namespace UnitTests.Tests.HappyTests;

public class HappyDatabaseTests
{
    /// <summary>
    /// This test ensures that SaveFavouriteStation successfully saves a favorite station to the database.
    /// </summary>
    [Fact]
    public void SaveFavouriteStation()
    {
        // Arrange
        var mockDatabase = new Mock<IDatabase>();

        // Set up the mock to return a list of favourite stations when GetFavouriteStations is called
        var favouriteStationsList = new List<DatabaseStation>()
        {
            new DatabaseStation
            {
                Id = 1,
                Naam = "Save Favourite Station Database Happy Test",
                StationType = "TEST_STATION",
                HeeftFaciliteiten = true,
                HeeftReisassistentie = true,
                Land = "NL",
                Lat = 0,
                Lng = 0,
                Distance = 26
            }
        };

        mockDatabase.Setup(m => m.GetFavouriteStations()).Returns(favouriteStationsList);

        mockDatabase.Setup(m => m.SaveFavouriteStation(It.IsAny<DatabaseStation>()))
            .Callback((DatabaseStation s) => favouriteStationsList.Add(s));

        var newStationToAddToDatabase = new DatabaseStation
        {
            Id = 2,
            Naam = "Station",
            StationType = "TEST_STATION2",
            HeeftFaciliteiten = false,
            HeeftReisassistentie = true,
            Land = "BE",
            Lat = 10,
            Lng = 22,
            Distance = 50
        };

        // Act
        mockDatabase.Object.SaveFavouriteStation(newStationToAddToDatabase);

        // Assert
        mockDatabase.Verify(m => m.SaveFavouriteStation(It.IsAny<DatabaseStation>()), Times.Once());
        Assert.Equal(2, mockDatabase.Object.GetFavouriteStations().Count);
    }

    /// <summary>
    /// This test ensures that DeleteFavouriteStationByName successfully deletes a favorite station by name from the database.
    /// Assuming there is a station named "Save Favourite Station Database Happy Test" in the database.
    /// </summary>
    [Fact]
    public void DeleteFavouriteStationByName()
    {
        Database database = new Database();
        //Assuming there is a station named "Save Favourite Station Database Happy Test" in the database
        string stationNameToDelete = "Test Station";

        database.DeleteFavouriteStationByName(stationNameToDelete);

        var savedStations = database.GetFavouriteStations();

        Assert.DoesNotContain(savedStations, station => station.Naam == stationNameToDelete);
    }
}