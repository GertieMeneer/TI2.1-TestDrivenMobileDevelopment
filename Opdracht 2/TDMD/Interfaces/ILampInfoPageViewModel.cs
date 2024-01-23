namespace TDMD.Interfaces
{
    public interface ILampInfoPageViewModel
    {
        Task OnApplyBrightness();
        Task ChangeLightColor();
        Task Toggle();
        double PercentageToValue(double percentage);
    }
}
