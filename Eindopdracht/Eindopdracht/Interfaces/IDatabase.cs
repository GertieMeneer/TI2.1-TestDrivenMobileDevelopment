using Eindopdracht.NSData;

namespace Eindopdracht.Interfaces
{
    public interface IDatabase
    {
        void SaveFavouriteStation(DatabaseStation station);
        List<DatabaseStation> GetFavouriteStations();
        void DeleteFavouriteStationByName(string stationName);
    }
}
