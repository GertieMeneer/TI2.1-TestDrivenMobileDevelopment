using TDMD.ApplicationLayer;

namespace TDMD.InfrastructureLayer;

public partial class LampInfoPage : ContentPage
{
	public LampInfoPage(LampInfoPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}