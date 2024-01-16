using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.Security;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TDMD
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;
        private string useriddingofzo;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            BindingContext = viewModel;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await GetUserIDAsync();

            if (Communicator.userid != null)
            {
                await LoadLamps();
            }
            else
            {
                DisplayAlert("error", "error", "Ok");
                viewModel.UserIDText = "error";
            }
        }

        private async Task GetUserIDAsync()
        {
            // before running the app click on the link button in the HUE emulator!!!
            if(await Communicator.GetUserIdAsync() == false)
            {
                DisplayAlert("Error", "Error getting UserID, maybe the server is not running or " +
                    "maybe link button not pressed?", "Ok");
                viewModel.UserIDText = "No UserID. Link button > refresh app";
            }
            else
            {
                viewModel.UserIDText = $"UserID: {Communicator.userid}";
            }

        }

        public async Task LoadLamps()
        {
            string url = $"http://192.168.1.179/api/" + Communicator.userid;


            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //when android phone: http://10.0.2.2:8000/api/newdeveloper
                    //when windows: http://192.168.1.179/api/newdeveloper
                    
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Status.Text = "Status: Connected!";
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Debug.WriteLine(jsonString);

                        viewModel.Lamps = LampParser.ParseLights(jsonString);
                    }
                    else
                    {
                        Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
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