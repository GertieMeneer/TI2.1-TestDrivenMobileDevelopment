using TDMD.Classes;
using TDMD.ViewModels;

namespace TDMD;

public partial class LampInfoPage : ContentPage
{
	public LampInfoPage(LampInfoPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}