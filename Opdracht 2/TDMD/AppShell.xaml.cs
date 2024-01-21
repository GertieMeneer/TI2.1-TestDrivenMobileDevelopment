namespace TDMD
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LampInfoPage), typeof(LampInfoPage));
        }
    }
}
