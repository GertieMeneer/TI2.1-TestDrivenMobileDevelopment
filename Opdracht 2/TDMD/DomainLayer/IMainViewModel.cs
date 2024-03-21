namespace TDMD.DomainLayer
{
    public interface IMainViewModel 
    {
        List<Lamp> Lamps { get; set; }

        Task Refresh();
        Task GoToLampInfoPage(Lamp lamp);
        Task RefreshData();
        void InitializeAsync();
        Task GetUserIDAsync();
        Task<string> LoadLamps();
        List<Lamp> ParseLights(string jsonResponse);
        double ValueToPercentage(double value);
        Task<string> GetUserIdAsync();
    }
}
