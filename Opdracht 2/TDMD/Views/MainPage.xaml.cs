using System.Diagnostics;
using TDMD.Classes;
using TDMD.ViewModels;


namespace TDMD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainViewModel viewModel = new MainViewModel();
            BindingContext = viewModel;
        }
    }
}