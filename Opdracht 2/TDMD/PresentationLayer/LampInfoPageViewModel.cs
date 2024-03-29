﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using TDMD.DomainLayer;
using CommunityToolkit.Maui.Core;

namespace TDMD.PresentationLayer
{
    [QueryProperty(nameof(Lamp), "Lamp")]
    public partial class LampInfoPageViewModel : ObservableObject, ILampInfoPageViewModel
    {
        private double _brightness;
        private double _brightnessPercentage;
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

        public double BrightnessPercentage
        {
            get { return _brightnessPercentage; }
            set
            {
                if (_brightnessPercentage != value)
                {
                    _brightnessPercentage = value;
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
            else
            {
                ShowLampOffError();
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
            else
            {
                ShowLampOffError();
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

        private async void ShowLampOffError()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = "Lamp is turned off";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        [ObservableProperty]
        Lamp lamp;
    }
}
