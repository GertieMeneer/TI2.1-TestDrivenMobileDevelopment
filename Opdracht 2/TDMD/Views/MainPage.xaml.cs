using System.Diagnostics;
using TDMD.Classes;
using TDMD.ViewModels;


namespace TDMD
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}