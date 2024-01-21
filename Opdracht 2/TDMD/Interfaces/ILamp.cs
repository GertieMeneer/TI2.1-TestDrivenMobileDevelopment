namespace TDMD.Interfaces
{
    public interface ILamp
    {
        Task ToggleLamp();
        Task SetBrightness(double value);
        Task SetColor(int hue, int sat);
    }
}
