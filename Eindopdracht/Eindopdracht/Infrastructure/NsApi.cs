using Eindopdracht.Domain;
using Newtonsoft.Json;

namespace Eindopdracht.Infrastructure
{
    public class NsApi : INsApi
    {
        private readonly string _nsapiKey = "12ef36ad08a1435597ae44c554d62ef8";
        private HttpClient _httpClient;

        public NsApi() { _httpClient = new HttpClient(); }

        /// <summary>
        /// Sends api request to NS api in order to get all the stations from the api.
        /// Then calculates the distance from device to station for each received station.
        /// </summary>
        /// <param name="latitude">y-coordinate of the device</param>
        /// <param name="longitude">x-coordinate of the device</param>
        /// <returns>List with all the NS Stations</returns>
        public async Task<List<NSStation>> GetAllNSStations(double latitude, double longitude)
        {
            List<NSStation> stations = new List<NSStation>();

            string nsStationsUrl = $"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/stations?latitude={latitude}&longitude={longitude}";
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _nsapiKey);

            var nsResponse = await _httpClient.GetStringAsync(nsStationsUrl);

            var nsStationsPayload = JsonConvert.DeserializeObject<NSStationPayload>(nsResponse);

            foreach (NSStation station in nsStationsPayload.Payload)
            {
                double stationLat = station.Lat;
                double stationLng = station.Lng;

                double distance = CalculateDistance(latitude, longitude, stationLat, stationLng);

                stations.Add(new NSStation
                {
                    Namen = station.Namen,
                    Distance = distance,
                    Lat = station.Lat,
                    Lng = station.Lng,
                    StationType = station.StationType,
                    HeeftFaciliteiten = station.HeeftFaciliteiten,
                    HeeftReisassistentie = station.HeeftReisassistentie,
                    Land = station.Land,
                });
            }
            return stations;
        }

        /// <summary>
        /// Calculates the distance between the device and a station
        /// </summary>
        /// <param name="userLat">y-coordinate of device</param>
        /// <param name="userLong">x-coordinate of device</param>
        /// <param name="stationLat">y-coordinate of station</param>
        /// <param name="stationLong">x-coordinate of station</param>
        /// <returns>The distance between the device and a station</returns>
        private double CalculateDistance(double userLat, double userLong, double stationLat, double stationLong)
        {
            double distance = Location.CalculateDistance(userLat, userLong, stationLat, stationLong, DistanceUnits.Kilometers);
            return distance;
        }
    }
}
