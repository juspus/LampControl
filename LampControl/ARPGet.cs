using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace LampControl
{
    public class ArpGet
    {
        public List<ArpEntity> GetArpResult()
        {
            var p = Process.Start(new ProcessStartInfo("arp", "-a")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            });

            var output = p?.StandardOutput.ReadToEnd();
            p?.Close();

            return ParseArpResult(output);
        }

        private List<ArpEntity> ParseArpResult(string output)
        {
            var lines = output.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l));

            var result =
                (from line in lines
                    select Regex.Split(line, @"\s+")
                        .Where(i => !string.IsNullOrWhiteSpace(i)).ToList()
                    into items
                    where items.Count == 3
                    select new ArpEntity()
                    {
                        Ip = items[0],
                        MacAddress = items[1],
                        Type = items[2]
                    });

            return result.ToList();
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }


}
}