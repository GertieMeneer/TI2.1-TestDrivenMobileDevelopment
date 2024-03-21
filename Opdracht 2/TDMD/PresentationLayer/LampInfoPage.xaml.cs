namespace TDMD.PresentationLayer;

public partial class LampInfoPage : ContentPage
{
	public LampInfoPage(LampInfoPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}