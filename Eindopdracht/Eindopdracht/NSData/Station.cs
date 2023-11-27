using SQLite;

namespace Eindopdracht.NSData
{
    [Table("NSStation")]
    public class Station
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Naam { get; set; }
        public double Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
