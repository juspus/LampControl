using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LampControl
{
    class Program
    {
        static void Main(string[] args)
        {
            var lightState = false;

            while (true)
            {
                ApiHelper.InitializeClient();
                var weather = WeatherProcessor.LoadWeather();
                // DateTime sunrise = UnixTimeStampToDateTime(weather.Result.Sys.Sunrise);
                DateTime sunset = UnixTimeStampToDateTime(weather.Result.Sys.Sunset);
                // var temperature = weather.Result.Main.Temperature;
                // var pressure = weather.Result.Main.Pressure;
                // var humidity = weather.Result.Main.Humidity;
                var clouds = weather.Result.Clouds.All;
                if ((System.DateTime.Now > sunset || clouds > 50) && !lightState)
                {
                    HttpWebRequest lightOn = (HttpWebRequest) WebRequest.Create("http://192.168.0.22/lightsOFF");
                    lightOn.Timeout = 10000;
                    HttpWebResponse lightOnResp = (HttpWebResponse) lightOn.GetResponse();
                    lightOn.Abort();

                    if (lightOnResp.StatusCode.ToString() == "OK") { lightState = true; }
                }
                else if (System.DateTime.Now <= sunset && clouds <= 50)
                {
                    HttpWebRequest lightOff = (HttpWebRequest) WebRequest.Create("http://192.168.0.22/lightsON");
                    lightOff.Timeout = 60000;
                    HttpWebResponse lightOffResp = (HttpWebResponse)lightOff.GetResponse();
                    lightOff.Abort();

                    if (lightOffResp.StatusCode.ToString() == "OK") { lightState = false; }
                }

                Thread.Sleep(6000);
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
