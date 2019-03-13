using System;
using System.Collections.Generic;
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
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += LampLogic;
        }

        static void LampLogic(object sender, ElapsedEventArgs e)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LampControlForm());
            var lightState = false;
            var counter = 0;
            //while (true)
            //{
                ApiHelper.InitializeClient();
                var weather = WeatherProcessor.LoadWeather();
                DateTime sunrise = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunrise);
                DateTime sunset = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunset).AddHours(-1);
                var clouds = weather.Result.Clouds.All;
                
                Console.Clear();
                Console.WriteLine("Cycle: " + (counter++) + " System Time: " + System.DateTime.Now);
                Console.WriteLine("Sunset: " + sunset);
                Console.WriteLine("Sunrise: " + sunrise);
                Console.WriteLine("Cloudiness: " + clouds);

                if ((System.DateTime.Now > sunset || clouds > 95 || System.DateTime.Now < sunrise) && !lightState)
                {
                    LampWebCommands.LightOn(out lightState, clouds, sunset);
                }
                else if (System.DateTime.Now >= sunrise && System.DateTime.Now <= sunset && clouds <= 95)
                {
                    LampWebCommands.LightOff(out lightState, clouds, sunset);
                }
                else
                {
                    Console.WriteLine("No Changes");
                }

                Console.WriteLine(" ");
                Thread.Sleep(1000);
            //}
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
