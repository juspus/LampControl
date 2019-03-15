using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace LampControl
{
    class Program
    {
        static void Main(string[] args) // Main method for running program as a service using Topshelf
        {
            var exitCode = HostFactory.Run(x =>
                {
                    x.Service<LampMain>(s =>
                    {
                        s.ConstructUsing(lamp => new LampMain());
                        s.WhenStarted(lamp => lamp.Start());
                        s.WhenStopped(lamp => lamp.Stop());
                    });
                    x.RunAsNetworkService();

                    x.SetServiceName("LampService");
                    x.SetDisplayName("Lamp Service");
                    x.SetDescription("This is the lamp service, used to control lamp via HTTP");
                });

            int exitCodeValue = (int) Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
