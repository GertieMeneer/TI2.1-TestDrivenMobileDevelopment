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
        }

        public async void LoadLamps()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //when android phone: http://10.0.2.2:8000/api/newdeveloper
                    //when windows: http://localhost:8000/api/newdeveloper
                    HttpResponseMessage response = await client.GetAsync("http://localhost:8000/api/newdeveloper");

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
    }
    }
