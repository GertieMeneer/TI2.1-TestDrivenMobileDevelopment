using Eindopdracht.Domain;

namespace Eindopdracht.ApplicationServices
{
    public class DatabaseService
    {
        private readonly IDatabase _database;

        public DatabaseService(IDatabase database)
        {
            _database = database;
        }

        public List<NSStation> getFavoriteStations()
        {
            List<NSStation> FavouriteStations = new List<NSStation>();
            foreach (DatabaseStation station in _database.GetFavouriteStations())
            {
                NSStation NSStation = new NSStation()
                {
                    Id = station.Id,
                    Distance = station.Distance,
                    Lat = station.Lat,
                    Lng = station.Lng,
                    Naam = station.Naam,
                    StationType = station.StationType,
                    HeeftFaciliteiten = station.HeeftFaciliteiten,
                    HeeftReisassistentie = station.HeeftReisassistentie,
                    Land = station.Land,
                    Namen = new NSStationNamen()
                    {
                        Lang = station.Naam,
                        Middel = "",
                        Kort = ""
                    }
                };
                FavouriteStations.Add(NSStation);
            }

            return FavouriteStations;
        }

        public List<DatabaseStation> getFavoriteStationsWithoutCast()
        {
            return _database.GetFavouriteStations();
        }

        public void SaveFavouriteStation(DatabaseStation station)
        {
            _database.SaveFavouriteStation(station);
        }

        public void DeleteFavouriteStationByName(string stationName)
        {
            _database.DeleteFavouriteStationByName(stationName);
        }
    }
}