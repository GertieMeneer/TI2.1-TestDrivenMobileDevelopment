using TDMD.Classes;
using TDMD.ViewModels;


namespace TDMD
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            BindingContext = viewModel;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            Lamp selectedLamp = (Lamp)e.SelectedItem;

            ((ListView)sender).SelectedItem = null;
            Navigation.PushAsync(new LampInfoPage(selectedLamp));
        }

        private async void OnToggleButtonClicked(object sender, EventArgs e)
        {
            if (Communicator.userid == null)
            {
                DisplayAlert("Error", "No UserID, cannot toggle lamps. Try pressing the link button and refreshing the app", "Ok");
            }
            else
            {
                var button = (Button)sender;
                var lamp = (Lamp)button.CommandParameter;
                await lamp.ToggleLamp();

                var lampInList = viewModel.Lamps.FirstOrDefault(l => l.ID == lamp.ID);
                if (lampInList != null)
                {
                    lampInList.Status = lamp.Status;
                }
            }
        }
    }
}