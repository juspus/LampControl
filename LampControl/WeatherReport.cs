using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LampControl
{
    public class WeatherReport
    {
        public float Temp { get; set; }
        public int Humidity { get; set; }
        public float Pressure { get; set; }

        public static double ConvertToCelcius(float temp)
        {
            float celcius = temp - (float) 273.15;
            return celcius;
        }

    }
}
