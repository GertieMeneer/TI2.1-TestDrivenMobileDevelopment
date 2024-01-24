namespace UnitTests.Tests.HappyTests;

public class HappyDatabaseTests
{
    /// <summary>
    /// This test ensures that SaveFavouriteStation successfully saves a favorite station to the database.
    /// </summary>
    [Fact]
    public void SaveFavouriteStation()
    {
        Database database = new Database();
        var station = new DatabaseStation
        {
            Distance = 26,
            Lat = 0,
            Lng = 0,
            Naam = "Save Favourite Station Database Happy Test",
            StationType = "TEST_STATION",
            HeeftFaciliteiten = true,
            HeeftReisassistentie = true,
            Land = "NL"
        };

        database.SaveFavouriteStation(station);

        var savedStations = database.GetFavouriteStations();

        Assert.True(savedStations[0].Naam == station.Naam);
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