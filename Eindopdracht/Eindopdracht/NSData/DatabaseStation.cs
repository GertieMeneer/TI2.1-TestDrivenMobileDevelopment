using SQLite;

namespace Eindopdracht.NSData
{
    [Table("NSStation")]
    public class DatabaseStation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Naam { get; set; }
        public string StationType { get; set; }
        public bool HeeftFaciliteiten { get; set; }
        public bool HeeftReisassistentie { get; set; }
        public string Land { get; set; }
    }
}
