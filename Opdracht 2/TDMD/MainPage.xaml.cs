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
        private string userid;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            BindingContext = viewModel;
            GetUserID();
            LoadLamps();
        }

        private async void GetUserID()
        {
            userid = await Communicator.GetUserID();
            DisplayAlert("yes", "works", "cancel");
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