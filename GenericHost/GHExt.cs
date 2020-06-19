using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHost
{
    public static class GHExt
    {
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, ServiceBaseLifetime>());
        }
        public static Task RunAsServiceAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseServiceBaseLifetime().Build().RunAsync(cancellationToken);
        }
        public static void GenerateInstallBatchs(string serviceName, string displayName)
        {
            install(serviceName, displayName);
            uninstall(serviceName);
            stop(serviceName);
            start(serviceName);
        }

        private static void writeFile(FileInfo batchFile, StringBuilder batchText)
        {
            using (var writer = new StreamWriter(batchFile.FullName, false, Encoding.GetEncoding("iso-8859-1"))) {
                writer.Write(batchText.ToString());
                writer.Flush();
            }
        }

        private static void start(string serviceName)
        {
            var batchFile = new FileInfo("start.bat");
            if (!batchFile.Exists || batchFile.Length == 0) {
                batchFile.Delete();

                StringBuilder batchText = new StringBuilder();
                batchText.AppendLine("REM Start service");
                batchText.AppendLine($"sc start {serviceName}");

                writeFile(batchFile, batchText);
            }
        }
        private static void stop(string serviceName)
        {
            var batchFile = new FileInfo("stop.bat");
            if (!batchFile.Exists || batchFile.Length == 0) {
                batchFile.Delete();

                StringBuilder batchText = new StringBuilder();
                batchText.AppendLine("REM Stop service");
                batchText.AppendLine($"sc stop {serviceName}");

                writeFile(batchFile, batchText);
            }
        }
        private static void uninstall(string serviceName)
        {
            var batchFile = new FileInfo("uninstall.bat");
            if (!batchFile.Exists || batchFile.Length == 0) {
                batchFile.Delete();

                StringBuilder batchText = new StringBuilder();
                batchText.AppendLine("REM Stop service");
                batchText.AppendLine($"sc stop {serviceName}");
                batchText.AppendLine();
                batchText.AppendLine("REM Delete service");
                batchText.AppendLine($"sc delete {serviceName}");

                writeFile(batchFile, batchText);
            }
        }
        private static void install(string serviceName, string displayName)
        {
            var batchFile = new FileInfo("install.bat");
            if (!batchFile.Exists || batchFile.Length == 0) {
                batchFile.Delete();

                string exeName = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");

                StringBuilder batchText = new StringBuilder();
                batchText.AppendLine("REM Install the service:");
                batchText.AppendLine($"sc create {serviceName} binPath= \"{exeName}\" start= auto DisplayName= \"{displayName}\"");
                batchText.AppendLine();
                batchText.AppendLine("REM Start the service");
                batchText.AppendLine($"sc start {serviceName}");

                writeFile(batchFile, batchText);
            }
        }
    }
}
