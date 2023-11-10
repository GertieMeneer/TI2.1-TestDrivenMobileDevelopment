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
                        type = type,
                        name = name,
                        modelid = id,
                        swversion = swversion,
                        uniqueid = uniqueid,

                        status = isOn,
                        brightness = brightness,
                        hue = hue,
                        sat = sat
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
    }
}
