using System.ComponentModel;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// Implements the IStatus interface. Reports status to the thread processing the current operation.
    /// </summary>
    public class Status : IStatus
    {
        private int _max;
        private int _currentStatus;
        private string _label;
        private BackgroundWorker _workHorse;

        public Status(BackgroundWorker workHorse)
        {
            _workHorse = workHorse;
        }

        public int CurrentStatus => _currentStatus;

        public string Label => _label;

        public void InitStatus(string label, int max)
        {
            _currentStatus = 0;
            _max = max;
            _label = label;
        }

        public void Increment()
        {
            _currentStatus++;
            double quotient = (double)_currentStatus / _max;
            try
            {
                _workHorse.ReportProgress((int)(quotient * 100));
            }
            catch { }
        }
    }
}
