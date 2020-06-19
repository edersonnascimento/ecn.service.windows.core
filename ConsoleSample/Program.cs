using GenericHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleExample
{
    /// <summary>
    /// https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            GHExt.GenerateInstallBatchs("ExampleService", "Generic host service sample");

            //This variable allows to execute at debug or console mode
            var isService = !(Debugger.IsAttached || args.Contains("--console"));


            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<SampleHostedService>();
                });

            if (isService) {
                try {
                    await builder.RunAsServiceAsync();
                } catch (InvalidOperationException) {
                    Console.WriteLine("Use the script 'install.bat' to install as a service.");
                }
            } else {
                Console.WriteLine("Use the script 'install.bat' to install as a service.");
                await builder.RunConsoleAsync();
            }

            Console.WriteLine();
        }
    }
}
