using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDMD.DomainLayer;

namespace TDMD.ApplicationLayer
{
    [QueryProperty(nameof(Lamp), "Lamp")]
    public partial class LampInfoPageViewModel : ObservableObject, ILampInfoPageViewModel
    {
        private double _brightness;
        private double _hue;
        private double _sat;

        public LampInfoPageViewModel() { }

        public double Brightness
        {
            get { return _brightness; }
            set
            {
                if (_brightness != value)
                {
                    _brightness = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Hue
        {
            get { return _hue; }
            set
            {
                if (_hue != value)
                {
                    _hue = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Sat
        {
            get { return _sat; }
            set
            {
                if (_sat != value)
                {
                    _sat = value;
                    OnPropertyChanged();
                }
            }
        }

        [RelayCommand]
        public async Task OnApplyBrightness()
        {
            if (lamp.Status != false)
            {
                double percentage = Brightness;

                await lamp.SetBrightness(PercentageToValue(percentage));
            }
        }

        [RelayCommand]
        public async Task ChangeLightColor()
        {
            if (lamp.Status != false)
            {
                int hue = (int)Hue;
                int sat = (int)Sat;

                await lamp.SetColor(hue, sat);
            }
        }

        [RelayCommand]
        public async Task Toggle()
        {
            await lamp.ToggleLamp();
        }

        public double PercentageToValue(double percentage)
        {
            double convertedValue = percentage / 100.0 * 253.0 + 1.0;
            return convertedValue;
        }

        [ObservableProperty]
        Lamp lamp;
    }
}
