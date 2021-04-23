using System;
using System.Diagnostics;


namespace MetricsAgent.interfaces
{
    public interface INotifier
    {
        void Notify();
        bool CanRun();
    }
    public class Notifier1 : INotifier
    {
        public bool CanRun()
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            Debug.WriteLine("Debugging from Notifier 1");
        }
    }
}
