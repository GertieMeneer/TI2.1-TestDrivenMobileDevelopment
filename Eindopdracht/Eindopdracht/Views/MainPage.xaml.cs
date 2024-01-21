using Eindopdracht.ViewModels;

namespace Eindopdracht
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            MainViewModel viewModel = new MainViewModel(new Database());
            BindingContext = viewModel;
        }
    }
}
