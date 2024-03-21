namespace Eindopdracht.Domain
{
    public interface IDatabase
    {
        void SaveFavouriteStation(DatabaseStation station);
        List<DatabaseStation> GetFavouriteStations();
        void DeleteFavouriteStationByName(string stationName);
    }
}
