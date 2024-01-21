using Eindopdracht.NSData;
using SQLite;

public static class Database
{
    private const string databasePath = "database.db";
    private static SQLiteConnection connection;

    static Database()
    {
        string applicationFolderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Folder");
        Directory.CreateDirectory(applicationFolderPath);

        string databaseFileName = Path.Combine(applicationFolderPath, databasePath);
        connection = new SQLiteConnection(databaseFileName);

        connection.CreateTable<DatabaseStation>();
    }
     
    public static void SaveFavouriteStation(DatabaseStation station)
    {
        connection.Insert(station);
    }

    public static List<DatabaseStation> GetFavouriteStations()
    {
        return connection.Table<DatabaseStation>().ToList();
    }

    public static void DeleteFavouriteStationByName(string stationName)
    {
        connection.Table<DatabaseStation>().Delete(s => s.Naam == stationName);
    }
}