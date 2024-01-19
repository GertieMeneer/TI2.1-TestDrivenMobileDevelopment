using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace TDMD.Classes
{
    public static class Communicator
    {
        public static List<Lamp> Lamps = new List<Lamp>();
        public static string userid;

        public static async Task<bool> GetUserIdAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"http://192.168.12.24/api";
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
                        userid = (string)successObject["username"];
                    }
                    catch
                    {
                        return false;
                    }

                    Debug.WriteLine($"User ID: {userid}");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
        }
    }
}
