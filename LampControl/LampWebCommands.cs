using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LampControl
{
    public class LampWebCommands
    {
        public static void LightOn()
        {
            HttpWebRequest lightOn = (HttpWebRequest)WebRequest.Create("http://192.168.0.22/lightsOFF");
            lightOn.Timeout = 10000;
            HttpWebResponse lightOnResp = (HttpWebResponse)lightOn.GetResponse();
            lightOn.Abort();
            if (lightOnResp.StatusCode.ToString() == "OK") { new LogWriter("LightOn "); }
        }

        public static void LightOff()
        {
            HttpWebRequest lightOff = (HttpWebRequest)WebRequest.Create("http://192.168.0.22/lightsON");
            lightOff.Timeout = 10000;
            HttpWebResponse lightOffResp = (HttpWebResponse)lightOff.GetResponse();
            lightOff.Abort();
            if (lightOffResp.StatusCode.ToString() == "OK") { new LogWriter("LightOff"); }
        }
    }
}
