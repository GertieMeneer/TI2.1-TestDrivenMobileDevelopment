using Eindopdracht.ViewModels;

namespace Eindopdracht
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Get an instance of the service provider
            var serviceProvider = MauiProgram.CreateMauiApp().Services;

            // Use dependency injection to obtain an instance of MainViewModel
            var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

            BindingContext = viewModel;
        }
    }
}
