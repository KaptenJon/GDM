using System;

namespace GDMInterfaces
{
    /// <summary>
    /// Interface for the type of plug-ins that manipulate the data.
    /// </summary>
    public interface ITool : IPlugin {

        /// <summary>
        /// If the plug-in need a column selected to be able to make settings.
        /// </summary>
        bool NeedColumnSelected { get; }

        /// <summary>
        /// If the plug-in need a table selected to be able to make settings.
        /// </summary>
        bool NeedTableSelected { get; }

        /// <summary>
        /// If the plug-in can operate on a column of the given data type.
        /// </summary>
        bool AcceptsDataType(Type t);

        /// <summary>
        /// A string describing the name of the category of plug-ins the plug-in belong to. All plug-ins with the same category name will be grouped together in the plug-in menu.
        /// </summary>
        string ToolCategory { get; }
    }
}
