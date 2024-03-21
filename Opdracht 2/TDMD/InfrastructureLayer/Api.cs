using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using TDMD.DomainLayer;

namespace TDMD.InfrastructureLayer
{
    public class Api : IApi
    {
        //when android phone: http://10.0.2.2:8000/
        //when windows: http://192.168.1.179/
        private string mainUrl = "http://10.0.2.2:80/api";

        private string userId;

        public Api() { }

        public async Task<string> LoadLamps()
        {
            string url = $"{mainUrl}" + "/" + userId;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Debug.WriteLine(jsonString);

                        return jsonString;
                    }
                    else
                    {
                        Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return string.Empty;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex);
                    return string.Empty;
                }
            }
        }

        public List<Lamp> ParseLights(string jsonResponse)
        {
            List<Lamp> lamps = new List<Lamp>();

            try
            {
                JObject jsonObject = JObject.Parse(jsonResponse);
                JObject lightsObject = jsonObject["lights"].ToObject<JObject>();

                foreach (var keyValuePair in lightsObject)
                {
                    string key = keyValuePair.Key;
                    JObject lightObject = keyValuePair.Value.ToObject<JObject>();

                    string id = key;
                    string name = lightObject["name"].ToString();
                    bool isOn = lightObject["state"]["on"].ToObject<bool>();
                    string type = lightObject["type"].ToString();
                    string swversion = lightObject["swversion"].ToString();
                    string uniqueid = lightObject["uniqueid"].ToString();
                    int brightness = lightObject["state"]["bri"].ToObject<int>();
                    int hue = lightObject["state"]["hue"].ToObject<int>();
                    int sat = lightObject["state"]["sat"].ToObject<int>();

                    Lamp lamp = new Lamp
                    {
                        ID = key,
                        Type = type,
                        Name = name,
                        ModelID = id,
                        SWVersion = swversion,
                        UniqueID = uniqueid,

                        Status = isOn,
                        Brightness = brightness,
                        BrightnessPercentage = ValueToPercentage(brightness),
                        Hue = hue,
                        Sat = sat
                    };

                    lamps.Add(lamp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return lamps;
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
                    return userId;
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return string.Empty;
                }
            }
        }

        private double ValueToPercentage(double value)
        {
            double percentage = value / 254.0 * 100.0;
            return Math.Round(percentage);
        }
    }
}
