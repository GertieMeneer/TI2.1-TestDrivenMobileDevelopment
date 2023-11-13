using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TDMD;

namespace TestProject.TDMD
{
    public class LampLoader
    {
        private ObservableCollection<Lamp> Lamps;

        public LampLoader()
        {
            Lamps = new ObservableCollection<Lamp>(new List<Lamp>());
        }

        public async Task LoadLamps()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //when android phone: http://10.0.2.2:8000/api/newdeveloper
                    //when windows: http://localhost:8000/api/newdeveloper
                    HttpResponseMessage response = await client.GetAsync("http://localhost:8000/api/newdeveloper");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Lamps = LightParser.ParseLights(jsonString);
                    }
                    else
                    {
                        Trace.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
        }

        public ObservableCollection<Lamp> GetLamps()
        {
            return Lamps;
        }
    }
}
