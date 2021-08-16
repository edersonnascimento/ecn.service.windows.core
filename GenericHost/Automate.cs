using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHost
{
    public class Automate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Timer _timer;
        private object _locker = new object();
        private static bool _stop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="automation">Classe que automatiza a tarefa</param>
        /// <param name="delay">Tempo (em segundos) entre os ciclos</param>
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
                if (Automation.State == IAutomation.eState.Waiting) {
                    Automation.Execute();
                }
            } catch (Exception e) {
                logger.Error(e);
            }
        }

        public void Start()
        {
            _stop = false;
            _timer = new Timer(
                e => executeAutomation(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(Delay)
           );
        }

        public static void Wait(CancellationToken cancellationToken)
        {
            Task.Run(() => {
                while (true) {
                    if (cancellationToken.IsCancellationRequested || _stop) {
                        break;
                    }
                    Task.Delay(1000);
                }
            }, cancellationToken)
            .Wait();
        }

        public void Stop()
        {
            _stop = true;
            Automation.Stop();
            _timer?.Change(Timeout.Infinite, 0);
        }
    }
}
