using Eindopdracht.Interfaces;
using Eindopdracht.NSData;
using SQLite;

public class Database : IDatabase
{
    private const string databasePath = "database.db";
    private SQLiteConnection connection;

    public Database()
    {
        string applicationFolderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Folder");
        Directory.CreateDirectory(applicationFolderPath);

        string databaseFileName = Path.Combine(applicationFolderPath, databasePath);
        connection = new SQLiteConnection(databaseFileName);

        connection.CreateTable<DatabaseStation>();
    }
     
    public void SaveFavouriteStation(DatabaseStation station)
    {
        connection.Insert(station);
    }

    public List<DatabaseStation> GetFavouriteStations()
    {
        return connection.Table<DatabaseStation>().ToList();
    }

    public void DeleteFavouriteStationByName(string stationName)
    {
        connection.Table<DatabaseStation>().Delete(s => s.Naam == stationName);
    }
}