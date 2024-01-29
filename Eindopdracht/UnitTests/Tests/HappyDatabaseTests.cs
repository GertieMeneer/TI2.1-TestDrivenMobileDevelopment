namespace UnitTests.Tests.HappyTests;

public class HappyDatabaseTests
{
    /// <summary>
    /// This test ensures that SaveFavouriteStation successfully saves a favorite station to the database.
    /// </summary>
    [Fact]
    public void SaveFavouriteStation()
    {
        //arrange
        var mockDatabase = new Mock<IDatabase>();

        //make the list of favourite stations
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

        //make the mock return a list of favourite stations when GetFavouriteStations is called
        mockDatabase.Setup(m => m.GetFavouriteStations()).Returns(favouriteStationsList);

        //make the callback so that the mock adds the station to the list
        mockDatabase.Setup(m => m.SaveFavouriteStation(It.IsAny<DatabaseStation>()))
            .Callback((DatabaseStation s) => favouriteStationsList.Add(s));

        //make the new database station
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

        //act
        mockDatabase.Object.SaveFavouriteStation(newStationToAddToDatabase);

        //assert
        mockDatabase.Verify(m => m.SaveFavouriteStation(It.IsAny<DatabaseStation>()), Times.Once());
        //check if the list of favourite stations has 2 objects in it
        Assert.Equal(2, mockDatabase.Object.GetFavouriteStations().Count);
    }

    /// <summary>
    /// This test ensures that DeleteFavouriteStationByName successfully deletes a favorite station by name from the database.
    /// Assuming there is a station named "Save Favourite Station Database Happy Test" in the database.
    /// </summary>
    [Fact]
    public void DeleteFavouriteStationByName()
    {
        //arrange
        var mockDatabase = new Mock<IDatabase>();

        //create a list of favourite stations
        var favoriteStationsList = new List<DatabaseStation>()
        {
            new DatabaseStation
            {
                Id = 1,
                Naam = "Station 1",
                StationType = "TEST_STATION2",
                HeeftFaciliteiten = false,
                HeeftReisassistentie = true,
                Land = "BE",
                Lat = 10,
                Lng = 22,
                Distance = 50
            },
            new DatabaseStation
            {
                Id = 2,
                Naam = "Station 2",
                StationType = "TEST_STATION2",
                HeeftFaciliteiten = false,
                HeeftReisassistentie = true,
                Land = "BE",
                Lat = 10,
                Lng = 22,
                Distance = 50
            },
            new DatabaseStation
            {
                Id = 3,
                Naam = "Test Station To Be Deleted",
                StationType = "TEST_STATION2",
                HeeftFaciliteiten = false,
                HeeftReisassistentie = true,
                Land = "BE",
                Lat = 10,
                Lng = 22,
                Distance = 50
            }
        };

        //set up the mock to return the list of favorite stations when GetFavoriteStations is called
        mockDatabase.Setup(m => m.GetFavouriteStations()).Returns(favoriteStationsList);

        //set up callback for mock when deletefavouritestationbyname is called to remove the station if the name equals to input parameter
        mockDatabase.Setup(m => m.DeleteFavouriteStationByName(It.IsAny<string>()))
            .Callback((string stationName) => favoriteStationsList.RemoveAll(station => station.Naam.Equals(stationName)));

        //act
        mockDatabase.Object.DeleteFavouriteStationByName("Test Station To Be Deleted");

        //assert
        mockDatabase.Verify(m => m.DeleteFavouriteStationByName("Test Station To Be Deleted"), Times.Once());
        Assert.Equal(2, mockDatabase.Object.GetFavouriteStations().Count);
    }
}