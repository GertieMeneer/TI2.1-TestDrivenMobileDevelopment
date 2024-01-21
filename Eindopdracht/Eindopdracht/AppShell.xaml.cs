namespace Eindopdracht
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(StationDetailPage), typeof(StationDetailPage));
        }
    }
}
