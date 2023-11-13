using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.Security;
using System.Text;

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

            LoadLamps();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // before running the app click on the link button in the HUE emulator!!!
            if(await Communicator.GetUserIdAsync() == false)
            {
                DisplayAlert("Error", "Error getting UserID, maybe the server is not running or " +
                    "maybe link button not pressed?", "Ok");
            }
            else
            {
                DisplayAlert("Info", "Connected successfully", "Ok");
            }

            await TurnOffLampAsync(1);
        }

        

        private async Task TurnOffLampAsync(int lampId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"http://10.0.2.2:8000/api/{Communicator.userid}/lights/{lampId}/state";
                string body = "{\"on\":false}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {lampId} turned off successfully.");
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        private async Task TurnOnLampAsync(int lampId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"http://10.0.2.2:8000/api/{Communicator.userid}/lights/{lampId}/state";
                string body = "{\"on\":true}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {lampId} turned on successfully.");
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        public async void LoadLamps()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //when android phone: http://10.0.2.2:8000/api/newdeveloper
                    //when windows: http://localhost:8000/api/newdeveloper
                    HttpResponseMessage response = await client.GetAsync("http://10.0.2.2:8000/api/newdeveloper");

                    // Ensure successful response before attempting to read content
                    if (response.IsSuccessStatusCode)
                    {
                        Status.Text = "Status: Connected!";
                        // Read the content as a string
                        string jsonString = await response.Content.ReadAsStringAsync();

                        // Now you can parse the JSON string into a JObject
                        // using Newtonsoft.Json as shown in the previous response
                        viewModel.Lamps = LightParser.ParseLights(jsonString);
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

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            // Handle item selection, e.g., show a pop-up or navigate to a new page
            Lamp selectedLamp = (Lamp)e.SelectedItem;
            selectedLamp.ToggleAsync();

            // Here, you can implement the logic to display a pop-up or navigate to a new page with more details
            DisplayAlert("Selected Item", $"Name: {selectedLamp.name}\nStatus: {selectedLamp.status}", "OK");

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
        }
    }
}