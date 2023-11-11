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
        private static string url = "http://10.0.2.2/api";

        public async static Task<string> GetUserID()
        {
            try
            {
                // Replace {uniqueid} and {status} with actual values

                HttpClient client = new HttpClient();

                // Prepare your JSON data
                JObject jObject = new JObject();
                jObject["devicetype"] = "my_device#gertiemeneer";
                var json = jObject.ToString();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and handle the response
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JArray jsonArray = JArray.Parse(responseContent);
                    JObject successObject = jsonArray[0]["success"] as JObject;

                    // Extract and store the username
                    string username = successObject["username"].ToString();

                    // Return the username or store it in a variable/class property
                    return username;
                }
                else
                {
                    // Handle the error, e.g., log or throw an exception
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
    }
}
