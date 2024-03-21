using Eindopdracht.ViewModels;

namespace Eindopdracht;

public partial class StationDetailPage : ContentPage
{
    public StationDetailPage(StationDetailViewModel stationDetailViewModel)
    {
        InitializeComponent();
        BindingContext = stationDetailViewModel;
        stationDetailViewModel.Initialize(map);
    }
}