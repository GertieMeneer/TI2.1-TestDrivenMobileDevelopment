using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace TDMD.Classes
{
    public class LampParser
    {
        public static List<Lamp> ParseLights(string jsonResponse)
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
                        BrightnessPercentage = Converter.ValueToPercentage(brightness),
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
    }
}
