using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LampControl
{
    public class WeatherReport
    {
        public float Temperature { get; set; }
        public int Humidity { get; set; }
        public float Pressure { get; set; }

    }
}
