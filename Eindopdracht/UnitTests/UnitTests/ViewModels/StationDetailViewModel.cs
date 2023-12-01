using System.Collections.ObjectModel;
using Eindopdracht.NSData;

namespace Eindopdracht.ViewModels
{
    public class StationDetailViewModel
    {
        public string StationType { get; }
        public bool HeeftFaciliteiten { get; }
        public bool HeeftReisassistentie { get; }
        public string Land { get; }
        public StationDetailViewModel(NSStation station)
        {
            StationType = station.StationType;
            HeeftFaciliteiten = station.HeeftFaciliteiten;
            HeeftReisassistentie = station.HeeftReisassistentie;
            Land = station.Land;
        }
    }
}
