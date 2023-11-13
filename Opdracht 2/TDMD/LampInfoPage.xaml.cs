namespace TDMD;

public partial class LampInfoPage : ContentPage
{
	private Lamp _lamp;
	public LampInfoPage(Lamp lamp)
	{
		InitializeComponent();
		_lamp = lamp;
        LampNameLabel.Text = lamp.name;
		if (lamp.status)
		{
			LampStatusLabel.Text = "Lamp status: ON";
        }
		else
		{
            LampStatusLabel.Text = "Lamp status: OFF";
        }

		LampBrightnessLabel.Text = $"Lamp Brightness: {ValueToPercentage(lamp.brightness)}%";
		LampHueLabel.Text = lamp.hue.ToString();
		LampSatLabel.Text = lamp.sat.ToString();

        LampIDLabel.Text = $"Lamp ID: {lamp.id}";
		LampTypeLabel.Text = $"Lamp type: {lamp.type}";
		LampModelIDLabel.Text = $"Lamp Model ID: {lamp.modelid}";
		LampSoftwareLabel.Text = $"Lamp Software: {lamp.swversion}";
		LampUniqueIDLabel.Text = $"Lamp Unique ID: {lamp.uniqueid}";
	}

	private double ValueToPercentage(double value)
	{
		double percentage = (value / 254.0) * 100.0;
		return percentage;
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
    }
}