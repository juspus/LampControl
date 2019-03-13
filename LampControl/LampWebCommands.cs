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
        public static void LightOn(out bool lightState, int clouds, DateTime sunset)
        {
            HttpWebRequest lightOn = (HttpWebRequest)WebRequest.Create("http://192.168.0.22/lightsOFF");
            lightOn.Timeout = 10000;
            HttpWebResponse lightOnResp = (HttpWebResponse)lightOn.GetResponse();
            lightOn.Abort();
            lightState = false;
            if (lightOnResp.StatusCode.ToString() == "OK")
            {
                lightState = true;
                Console.WriteLine("LightOn " + clouds + " " + DateTime.Now + " " + sunset);
            }
        }

        public static void LightOff(out bool lightState, int clouds, DateTime sunset)
        {
            HttpWebRequest lightOff = (HttpWebRequest)WebRequest.Create("http://192.168.0.22/lightsON");
            lightOff.Timeout = 10000;
            HttpWebResponse lightOffResp = (HttpWebResponse)lightOff.GetResponse();
            lightOff.Abort();
            lightState = true;
            if (lightOffResp.StatusCode.ToString() == "OK")
            {
                lightState = false;
                Console.WriteLine("LightOff");
            }
        }
    }
}
