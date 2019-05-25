using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDMInterfaces
{
    public class PluginException : Exception
    {
        public PluginException(string message)
        {
            ErrorMessage = message;
        }
        public String ErrorMessage { get; set; }
    }
    /// <summary>
    /// Base class of all plug-in settings classes. Contains a validation method that must be overriden by all settings classes.
    /// </summary>
    /// <remarks>Is implemented as an bastract class instead of an interface due to a limitation in the .NET Xml Serialization classes, that is used to store the settings made for a plug-in.</remarks>
    public abstract class PluginSettings
    {
        public static PluginSettings CopyAndChangeParameter(PluginSettings setting, string parameterToExchange, string newvalue, Type[] plugintypes)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                XmlSerializer ser = new XmlSerializer(typeof (PluginSettings), plugintypes);
                ser.Serialize(ms, setting);
                ms.Position = 0;
                var reader = new StreamReader(ms);
                var xml = reader.ReadToEnd();
                reader.Close();
                xml = xml.Replace(parameterToExchange, newvalue);
                PluginSettings t =
                    (PluginSettings) ser.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(xml ?? "")));
                return t;
            }
        }
        protected static TaskFactory<bool> Factory = new TaskFactory<bool>();
        /// <summary>
        /// Tests if the current state of the settings object is valid as input to the Apply method of the corresponding plug-in.
        /// </summary>
        /// <returns>a bool saying whether the validation was successful or not</returns>
        public abstract Task<bool> IsValid();

       public static bool IsInUIMode
        { get; set; }

        public static DateTime LockPlugin { get; set; }
    }
}
