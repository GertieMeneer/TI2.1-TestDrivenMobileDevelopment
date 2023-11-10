using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMD
{
    public class Lamp
    {
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string swversion { get; set; }
        public string uniqueid { get; set; }

        public bool status { get; set; }
        public int brightness { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }
    
    }
}
