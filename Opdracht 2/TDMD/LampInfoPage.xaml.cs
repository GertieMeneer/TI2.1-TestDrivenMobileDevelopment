
namespace TDMD;

public partial class LampInfoPage : ContentPage
{
	private Lamp _lamp;
	public LampInfoPage(Lamp lamp)
	{
		InitializeComponent();
		_lamp = lamp;
        LampNameLabel.Text = lamp.Name;
		if (lamp.Status)
		{
			LampStatusLabel.Text = "Lamp status: ON";
			LampStatusLabel.TextColor = Color.FromHex("#75fa28");
        }
		else
		{
            LampStatusLabel.Text = "Lamp status: OFF";
			LampStatusLabel.TextColor = Color.FromHex("#fc0317");
        }

		LampBrightnessLabel.Text = $"Lamp Brightness: {Converter.ValueToPercentage(lamp.Brightness)}%";
		BrightnessSlider.Value = Converter.ValueToPercentage(lamp.Brightness);
		LampHueLabel.Text = $"Lamp Hue: {lamp.Hue}";
		hueSlider.Value = lamp.Hue;
		saturationSlider.Value = Convert.ToInt32(lamp.Sat);
		LampSatLabel.Text = $"Lamp Saturation: {lamp.Sat}";

        LampIDLabel.Text = $"Lamp ID: {lamp.ID}";
		LampTypeLabel.Text = $"Lamp type: {lamp.Type}";
		LampModelIDLabel.Text = $"Lamp Model ID: {lamp.ModelID}";
		LampSoftwareLabel.Text = $"Lamp Software: {lamp.SWVersion}";
		LampUniqueIDLabel.Text = $"Lamp Unique ID: {lamp.UniqueID}";
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

    }

    private async void OnApplyBrightnessClick(Object sender, EventArgs e)
	{
        if (_lamp.Status == false)
        {
            DisplayAlert("Error", "Lamp is turned off. Cannot change color", "Ok");
        }
        else if (Communicator.userid == null)
        {
            DisplayAlert("Error", "No UserID. Cannot change color. Please press the link button and refresh the list of lamps", "Ok");
        }
        else
		{
            double percentage = BrightnessSlider.Value;
            double value = Converter.PercentageToValue(percentage);

            await _lamp.SetBrightness(value);

            LampBrightnessLabel.Text = $"Lamp Brightness: {Converter.ValueToPercentage(_lamp.Brightness)}%";

        }
    }

	private async void ChangeLightColor_Clicked(Object sender, EventArgs e)
	{
		if(_lamp.Status == false)
		{
			DisplayAlert("Error", "Lamp is turned off. Cannot change brightness", "Ok");
		}
		else if(Communicator.userid == null)
		{
			DisplayAlert("Error", "No UserID. Cannot change brightness. Please press the link button and refresh the list of lamps", "Ok");
		}
		else
		{
            int hue = (int)hueSlider.Value;
            int sat = (int)saturationSlider.Value;

            await _lamp.SetColor(hue, sat);

            LampHueLabel.Text = $"Lamp Hue: {_lamp.Hue}";
            LampSatLabel.Text = $"Lamp Saturation: {_lamp.Sat}";
        }
    }
}