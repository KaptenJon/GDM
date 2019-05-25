namespace GDMInterfaces
{
    /// <summary>
    /// Exposes methods that let the plug-in developer control the status messages shown and the progress of the status bar.
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Returns the current position of the status bar.
        /// </summary>
        int CurrentStatus
        {
            get;
        }

        /// <summary>
        /// Set the label and the maximum count of the status bar.
        /// </summary>
        /// <remarks>The plug-in must call the Increment() method exactly the number of times defined by max for the status bar to display 100%.</remarks>
        void InitStatus(string label, int max);
        /// <summary>
        /// Increments the status progress counter by one. (Towards the initialized maximum.)
        /// </summary>
        void Increment();
    }
}