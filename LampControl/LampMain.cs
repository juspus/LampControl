using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Serialization;
using Timer = System.Timers.Timer;

namespace LampControl
{
    public class LampMain
    {
        private readonly Timer _timer;
        public LampMain()
        {
            _timer = new Timer(10000) {AutoReset = true};
            _timer.Elapsed += LampLogic;
        }

        static void LampLogic(object sender, ElapsedEventArgs e)
        {
            ApiHelper.InitializeClient();
            Console.SetWindowSize(35, 6);
            Console.Clear();
            var weather = WeatherProcessor.LoadWeather();
            DateTime sunrise = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunrise);
            DateTime sunset = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunset).AddHours(-1);
            var clouds = weather.Result.Clouds.All;

            if (!ArpGet.PingHost("192.168.0.88"))
            {
                Console.WriteLine("Neranda IP");
                LampWebCommands.LightOff();
                return;
            }

            //List<ArpEntity> arpEntities = new ArpGet().GetArpResult();
            //var ip = arpEntities.FirstOrDefault(a => a.MacAddress == "64-a2-f9-2f-fb-9b")?.Ip;
            //if (ip == null)
            //{
            //    Console.WriteLine("Neranda IP");
            //    LampWebCommands.LightOff(clouds, sunset);
            //    return;
            //}
            
            Console.WriteLine("System Time: " + System.DateTime.Now);
            Console.WriteLine("Sunset: " + sunset);
            Console.WriteLine("Sunrise: " + sunrise);
            Console.WriteLine("Cloudiness: " + clouds);

            if (System.DateTime.Now > sunset || clouds > 95 || System.DateTime.Now < sunrise)
            {
                LampWebCommands.LightOn();
                //string[] on = new string[]{"on"};
                //File.AppendAllLines(@"C:\Temp\Demos\log.txt", on);
            }
            else if (System.DateTime.Now >= sunrise && System.DateTime.Now <= sunset && clouds <= 95)
            {
                LampWebCommands.LightOff();
                //string[] off = new string[] { "off" };
                //File.AppendAllLines(@"C:\Temp\Demos\log.txt", off);
            }
            //string[] lines = new string[] { "System Time: " + System.DateTime.Now + "\n" + "Sunset: " + sunset + "\n" + "Sunrise: " + sunrise + "\n" + "Cloudiness: " + clouds + "\n" };
            //File.AppendAllLines(@"C:\Temp\Demos\log.txt", lines);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

    }
}
