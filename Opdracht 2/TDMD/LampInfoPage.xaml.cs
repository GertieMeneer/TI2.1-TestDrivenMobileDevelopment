namespace TDMD;

public partial class LampInfoPage : ContentPage
{
	public LampInfoPage(Lamp lamp)
	{
		InitializeComponent();
		LampNameLabel.Text = lamp.name;
		LampIDLabel.Text = lamp.id;
	}
}