using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TDMD
{
    public class LightParser
    {
        public static ObservableCollection<Lamp> ParseLights(string jsonResponse)
        {
            ObservableCollection<Lamp> lamps = new ObservableCollection<Lamp>();

            try
            {
                JObject jsonObject = JObject.Parse(jsonResponse);
                JObject lightsObject = jsonObject["lights"].ToObject<JObject>();

                // Itereer door elke lamp
                foreach (var keyValuePair in lightsObject)
                {
                    string key = keyValuePair.Key;
                    JObject lightObject = keyValuePair.Value.ToObject<JObject>();

                    // Haal lamp eigenschappen op
                    string id = key;
                    string name = lightObject["name"].ToObject<string>();
                    bool isOn = lightObject["state"]["on"].ToObject<bool>();

                    // Maak een Lamp object aan
                    Lamp lamp = new Lamp
                    {
                        modelid = id,
                        name = name,
                        status = isOn
                    };

                    // Voeg de lamp toe aan de lijst
                    lamps.Add(lamp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return lamps;
        }
    }
}
