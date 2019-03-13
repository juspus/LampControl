using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LampControl
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LampControlForm());
            var lightState = false;
            var counter = 0;
            while (true)
            {
                ApiHelper.InitializeClient();
                var weather = WeatherProcessor.LoadWeather();
                DateTime sunrise = UnixTimeStampToDateTime(weather.Result.Sys.Sunrise);
                DateTime sunset = UnixTimeStampToDateTime(weather.Result.Sys.Sunset);
                var temperature = WeatherReport.ConvertToCelcius(weather.Result.Main.Temp);
                var pressure = weather.Result.Main.Pressure;
                var humidity = weather.Result.Main.Humidity;
                var clouds = weather.Result.Clouds.All;
                sunset = sunset.AddHours(-1);
                
                Console.Clear();
                Console.WriteLine("Cycle: " + (counter++) + " System Time: " + System.DateTime.Now);
                Console.WriteLine("Sunset: " + sunset);
                Console.WriteLine("Sunrise: " + sunrise);
                Console.WriteLine("Temperature: " + temperature);
                Console.WriteLine("Pressure: " + pressure);
                Console.WriteLine("Humidity: " + humidity);
                Console.WriteLine("Cloudiness: " + clouds);
                

                if ((System.DateTime.Now > sunset || clouds > 95 || System.DateTime.Now < sunrise) && !lightState)
                {
                    HttpWebRequest lightOn = (HttpWebRequest) WebRequest.Create("http://192.168.0.22/lightsOFF");
                    lightOn.Timeout = 10000;
                    HttpWebResponse lightOnResp = (HttpWebResponse) lightOn.GetResponse();
                    lightOn.Abort();

                    if (lightOnResp.StatusCode.ToString() == "OK") { lightState = true;
                        Console.WriteLine("LightOn " + clouds + " " + DateTime.Now + " " + sunset);
                    }
                }
                else if (System.DateTime.Now >= sunrise && System.DateTime.Now <= sunset && clouds <= 95)
                {
                    HttpWebRequest lightOff = (HttpWebRequest) WebRequest.Create("http://192.168.0.22/lightsON");
                    lightOff.Timeout = 10000;
                    HttpWebResponse lightOffResp = (HttpWebResponse)lightOff.GetResponse();
                    lightOff.Abort();

                    if (lightOffResp.StatusCode.ToString() == "OK")
                    {
                        lightState = false;
                        Console.WriteLine("LightOff");
                    }
                }
                Console.WriteLine(" ");
                Thread.Sleep(1000);
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
