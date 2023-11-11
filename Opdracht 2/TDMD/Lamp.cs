using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMD
{
    public class Lamp
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string swversion { get; set; }
        public string uniqueid { get; set; }

        public bool status { get; set; }
        public int brightness { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }

        public async Task ToggleAsync()
        {
            try
            {
                // Replace "your_api_endpoint" with the actual API endpoint you want to call
                string apiUrl = $"http://10.0.2.2:8000/api/f5495a1e5e2322e137a2e220a696e4c/lights/{id}/state";

                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    bool newState = !status;
                    // Create the content to be sent in the request (you may need to adjust this based on your API)
                    string requestBody = $"{{\"on\": {newState}}}";
                    HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

                    // Make the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        // Process the response content as needed
                        Debug.WriteLine(responseContent);
                        status = newState;
                    }
                    else
                    {
                        // Handle the error case
                        Debug.WriteLine($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }
    
    }
}
