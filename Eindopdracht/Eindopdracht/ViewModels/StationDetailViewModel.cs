using System.Collections.ObjectModel;
using Eindopdracht.NSData;

namespace Eindopdracht.ViewModels
{
    public class StationDetailViewModel
    {
        public ObservableCollection<NSStation> Station { get; set; }
        public StationDetailViewModel(NSStation station)
        {
            Station = new ObservableCollection<NSStation>()
            {
                new NSStation()
                {
                    StationType = station.StationType,
                    HeeftFaciliteiten = station.HeeftFaciliteiten,
                    HeeftReisassistentie = station.HeeftReisassistentie,
                    Land = station.Land
                }
            };
        }
    }
}
