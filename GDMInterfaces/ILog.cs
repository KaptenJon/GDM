namespace GDMInterfaces
{
    /// <summary>
    /// Exposes method to add messages to the application log.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Adds a line to the application log.
        /// </summary>
        void Add(LogType type, string message);
    }
    /// <summary>
    /// Used by ILog to determine what type of message that is written to the application log.
    /// </summary>
    public enum LogType { 
        /// <summary>
        /// Execution can continue but an abnormality was found. 
        /// </summary>
        Warning, 
        /// <summary>
        /// Execution can not continue an error occured.
        /// </summary>
        Error, 
        /// <summary>
        /// Execution terminated successfully.
        /// </summary>
        Success, 
        /// <summary>
        /// A note.
        /// </summary>
        Note }
}