namespace TDMD.DomainLayer
{
    public interface ILamp
    {
        string ID { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        string ModelID { get; set; }
        string SWVersion { get; set; }
        string UniqueID { get; set; }
        bool Status { get; set; }
        double Brightness { get; set; }
        double BrightnessPercentage { get; set; }
        int Hue { get; set; }
        int Sat { get; set; }

        Task ToggleLamp();
        Task SetBrightness(double value);
        Task SetColor(int hue, int sat);
        Task<string> GetUserIdAsync();
    }
}
