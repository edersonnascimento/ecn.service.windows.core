namespace GenericHost
{
    public interface IAutomation
    {
        public enum eState
        {
            Waiting,
            Executing
        }

        eState State { get; }
        void Execute();
        void Stop();
    }
}
