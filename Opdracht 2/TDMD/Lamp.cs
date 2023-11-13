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

        
        public async Task ToggleLamp()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                bool otherState = !status;
                string url = $"http://10.0.2.2:8000/api/{Communicator.userid}/lights/{id}/state";
                string body = $"{{\"on\":{otherState.ToString().ToLower()}}}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {id} turned on successfully.");
                    status = otherState;

                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
