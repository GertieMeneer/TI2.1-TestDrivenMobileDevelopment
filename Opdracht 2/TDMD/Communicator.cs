using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMD
{
    public static class Communicator
    {
        public static List<Lamp> Lamps = new List<Lamp>();
        public static string userid;

        public static async Task<bool> GetUserIdAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"http://localhost:8000/api";
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
