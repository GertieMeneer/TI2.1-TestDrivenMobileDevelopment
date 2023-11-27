namespace Eindopdracht.NSData
{
    public class NSStation
    {
        public string Naam { get; set; }
        public double Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public NSStationNamen Namen { get; set; }
        public string StationType { get; set; }
        public bool HeeftFaciliteiten { get; set; }
        public bool HeeftReisassistentie { get; set; }
        public string Land { get; set; }
    }
}
