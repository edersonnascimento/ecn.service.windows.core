using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleExample
{
    public class SampleHostedService : IHostedService, IDisposable
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        protected static bool _stop;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.Debug("Service starting!");
            while (!_stop) {
                logger.Debug("Do action");
                Thread.Sleep(1000);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Debug("Service stopping!");
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            logger.Debug("Dispose Called!");
        }
    }
}
