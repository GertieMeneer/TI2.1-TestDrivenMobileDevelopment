namespace TDMD;

public partial class LampInfoPage : ContentPage
{
	public LampInfoPage(Lamp lamp)
	{
		InitializeComponent();
		LampNameLabel.Text = lamp.name;
		if (lamp.status)
		{
			LampStatusLabel.Text = "Lamp status: ON";
        }
		else
		{
            LampStatusLabel.Text = "Lamp status: OFF";
        }

		LampBrightnessLabel.Text = $"Lamp Brightness: {ConvertBrightness(lamp.brightness)}";
		LampHueLabel.Text = lamp.hue.ToString();
		LampSatLabel.Text = lamp.sat.ToString();

        LampIDLabel.Text = $"Lamp ID: {lamp.id}";
		LampTypeLabel.Text = $"Lamp type: {lamp.type}";
		LampModelIDLabel.Text = $"Lamp Model ID: {lamp.modelid}";
		LampSoftwareLabel.Text = $"Lamp Software: {lamp.swversion}";
		LampUniqueIDLabel.Text = $"Lamp Unique ID: {lamp.uniqueid}";
	}

	private int ConvertBrightness(int value)
	{
		int percentage = (value / 254) * 100;
		return percentage;
	}
}