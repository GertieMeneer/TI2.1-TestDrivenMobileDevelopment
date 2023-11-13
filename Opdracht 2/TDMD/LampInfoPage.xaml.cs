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

		LampBrightnessLabel.Text = $"Lamp Brightness: {ValueToPercentage(lamp.Brightness)}%";
		BrightnessSlider.Value = ValueToPercentage(lamp.Brightness);
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

	private double ValueToPercentage(double value)
	{
		double percentage = (value / 254.0) * 100.0;
		return Math.Round(percentage);
	}

	private double PercentageToValue(double percentage)
	{
        double convertedValue = (percentage / 100.0) * 253.0 + 1.0;
		return convertedValue;
    }

	private async void OnApplyBrightnessClick(Object sender, EventArgs e)
	{
		double percentage = BrightnessSlider.Value;
		double value = PercentageToValue(percentage);

		await _lamp.SetBrightness(value);

        LampBrightnessLabel.Text = $"Lamp Brightness: {ValueToPercentage(_lamp.Brightness)}%";
    }

	private async void ChangeLightColor_Clicked(Object sender, EventArgs e)
	{
		int hue = (int)hueSlider.Value;
		int sat = (int)saturationSlider.Value;

		await _lamp.SetColor(hue, sat);

        LampHueLabel.Text = $"Lamp Hue: {_lamp.Hue}";
        LampSatLabel.Text = $"Lamp Saturation: {_lamp.Sat}";
    }
}