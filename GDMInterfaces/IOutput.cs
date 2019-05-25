namespace GDMInterfaces
{
    /// <summary>
    /// Interface for the type of plug-ins that produce output from the program.
    /// </summary>
    public interface IOutput : IIo 
    {
        /// <summary>
        /// Return the set of Tags exposed by this plug-in. The reference given of the Tag type is populated by the application when the user assigns Tags to the data.
        /// </summary>
        Tag Tags { get; }
    }
}
