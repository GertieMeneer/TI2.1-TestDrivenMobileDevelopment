using CommunityToolkit.Mvvm.ComponentModel;
using Eindopdracht.NSData;
using System.Diagnostics;

namespace Eindopdracht.ViewModels
{
    [QueryProperty(nameof(Station), "s")]
    public partial class StationDetailViewModel : ObservableObject
    {
        public StationDetailViewModel() { }

        [ObservableProperty]
        NSStation station;
    }
}
