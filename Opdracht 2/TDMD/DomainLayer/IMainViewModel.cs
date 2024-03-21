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
    }
}