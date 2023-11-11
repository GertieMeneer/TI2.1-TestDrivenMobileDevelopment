
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Opdracht_1
{
    public partial class MainPage : ContentPage
    {
        private List<Double> data = new List<Double>();
        private HttpClient httpClient = new HttpClient();
        private string url = "https://bloodservice.azurewebsites.net/bloodsugar";

        public MainPage()
        {
            InitializeComponent();
            GetValues();
        }

        public async void GetValues()
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await httpClient.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                // Assuming the response body is a JSON array of objects.
                string responseBody = await response.Content.ReadAsStringAsync();

                // Use JArray from Newtonsoft.Json to parse the JSON array.
                JArray responseData = JArray.Parse(responseBody);

                // Extract values from each object in the array.
                foreach (JObject item in responseData)
                {
                    double value = (double)item["value"];
                    // Assuming "value" is the property you want to add to the data list. je bent gay lol <3
                    data.Add(value);
                }

                // Assuming ValueList is a UI control, update its ItemsSource.
                ValueList.ItemsSource = data;
            }
            else
            {
                // Handle the case where the request was not successful.
                Trace.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
