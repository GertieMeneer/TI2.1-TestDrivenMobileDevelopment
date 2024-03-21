using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TDMD.DomainLayer
{
    public class Lamp : INotifyPropertyChanged
    {
        public Lamp()
        {
            GetUserIdAsync();
        }

        private string mainUrl = "http://10.0.2.2:80/api";

        private bool _status;
        private double _brightness;
        private string userId;
        public string ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ModelID { get; set; }
        public string SWVersion { get; set; }
        public string UniqueID { get; set; }

        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        public double Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                OnPropertyChanged();
            }
        }
        public double BrightnessPercentage { get; set; }
        public int Hue { get; set; }
        public int Sat { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task ToggleLamp()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                bool otherState = !Status;
                string url = $"{mainUrl}/{userId}/lights/{ID}/state";
                string body = $"{{\"on\":{otherState.ToString().ToLower()}}}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {ID} turned on successfully.");
                    Status = otherState;
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        public async Task SetBrightness(double value)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"{mainUrl}/{userId}/lights/{ID}/state";
                string body = $"{{\"bri\":{value}}}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {ID} changed brightness.");
                    Brightness = Math.Round(value / 254.0 * 100.0);
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        public async Task SetColor(int hue, int sat)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"{mainUrl}/{userId}/lights/{ID}/state";
                string body = $"{{\"hue\": {hue}, \"sat\": {sat}}}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Lamp {ID} turned on successfully.");
                    Hue = hue;
                    Sat = sat;
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        public async Task<string> GetUserIdAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = mainUrl;
                string body = "{\"devicetype\":\"my_hue_app#gertiemeneer\"}";

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    try
                    {
                        JArray jsonArray = JArray.Parse(result);
                        JObject successObject = jsonArray[0]["success"] as JObject;
                        userId = (string)successObject["username"];
                    }
                    catch
                    {
                        return string.Empty;
                    }

                    Debug.WriteLine($"User ID: {userId}");
                    return "userId";
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return string.Empty;
                }
            }
        }
    }
}
