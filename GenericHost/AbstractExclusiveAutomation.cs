using NLog;

namespace GenericHost
{
    public abstract class AbstractExclusiveAutomation : IAutomation
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        protected static bool _stop;
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
        public virtual void Stop()
        {
            _stop = true;
        }
    }
}
