using System;
using System.Collections.Generic;
using Eindopdracht.NSData;
using SQLite;

public class Database
{
    private const string databasePath = "database.db";
    private SQLiteConnection connection;

    public Database()
    {
        string applicationFolderPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Folder");
        // Create the folder path.
        System.IO.Directory.CreateDirectory(applicationFolderPath);

        string databaseFileName = System.IO.Path.Combine(applicationFolderPath, databasePath);
        connection = new SQLiteConnection(databaseFileName);

        // Create the Station table if it doesn't exist
        connection.CreateTable<Station>();
    }
     
    public void SaveStation(Station station)
    {
        connection.Insert(station);
    }

    public List<Station> GetStations()
    {
        return connection.Table<Station>().ToList();
    }

    public void DeleteStationByName(string stationName)
    {
        connection.Table<Station>().Delete(s => s.Naam == stationName);
    }
}