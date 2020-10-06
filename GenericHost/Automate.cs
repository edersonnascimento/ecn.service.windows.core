using NLog;
using System;
using System.Threading;

namespace GenericHost
{
    public class Automate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Timer _timer;
        private object _locker = new object();

        public Automate(IAutomation automation, int delay)
        {
            Automation = automation;
            Delay = delay;
        }
        public IAutomation Automation { get; private set; }
        /// <summary>
        /// Espaço de tempo entre as execuções, em segundos
        /// </summary>
        public int Delay { get; private set; }

        private void executeAutomation()
        {
            try {
                if(Automation.State == IAutomation.eState.Waiting) {
                    Automation.Execute();
                }
            } catch (Exception e) {
                logger.Error(e);
            }
        }

        public void Start() =>
            _timer = new Timer(
                e => executeAutomation(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(Delay)
           );

        public void Stop()
        {
            Automation.Stop();
            _timer?.Change(Timeout.Infinite, 0);
        }
    }
}
