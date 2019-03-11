using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampControl
{
    public class WeatherWeather
    {
        public CloudReport Clouds { get; set; }
        public WeatherReport  Main{ get; set; }
        public SunReport Sys { get; set; }
    }
}
