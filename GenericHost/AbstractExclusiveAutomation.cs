using NLog;

namespace GenericHost
{
    public abstract class AbstractExclusiveAutomation : IAutomation
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        public IAutomation.eState State { get; private set; }

        protected abstract void ExclusiveExecute();
        
        public void Execute()
        {
            if (State == IAutomation.eState.Waiting) {
                _logger.Trace($"Starting {this.GetType().Name} Process");
                try {
                    State = IAutomation.eState.Executing;
                    ExclusiveExecute();
                } finally {
                    State = IAutomation.eState.Waiting;
                    _logger.Trace($"Ending {this.GetType().Name} Process");
                }
            }
        }
        public abstract void Stop();
    }
}
