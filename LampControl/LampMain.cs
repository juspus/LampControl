using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
            new LogWriter("\n" + "*******************************" + "\n" + "Task Started" + "\n" +
                          "*******************************");
            ApiHelper.InitializeClient();
            new LogWriter( "Api Client Initialized");
            
            var weather = WeatherProcessor.LoadWeather();
            DateTime sunrise = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunrise);
            DateTime sunset = SunReport.UnixTimeStampToDateTime(weather.Result.Sys.Sunset).AddHours(-1);
            var clouds = weather.Result.Clouds.All;
            new LogWriter("Api Values got");

            if (!ArpGet.PingHost("192.168.0.88"))
            {
                new LogWriter("IP Not Found/Couldn't ping");
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
            
            new LogWriter("Sunset: " + sunset );
            new LogWriter("Sunrise: " + sunrise);
            new LogWriter("Cloudiness: " + clouds);

            if (System.DateTime.Now > sunset || clouds > 95 || System.DateTime.Now < sunrise)
            {
                LampWebCommands.LightOn();
            }
            else if (System.DateTime.Now >= sunrise && System.DateTime.Now <= sunset && clouds <= 95)
            {
                LampWebCommands.LightOff();
            }
        }

        public void Start()
        {
            _timer.Start();
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                File.Create(m_exePath + "\\" + "LampControl.log").Close();
            }
            catch (Exception ex)
            {
            }
            
            new LogWriter("\n" + "*******************************" + "\n" + "Service STARTED successfully" + "\n" +
                          "*******************************");
        }

        public void Stop()
        {
            _timer.Stop();
            new LogWriter("\n" + "*******************************" + "\n" + "Service STOPPED successfully" + "\n" +
                          "*******************************");
        }
    }
}
