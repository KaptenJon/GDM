using System;
using System.Collections.Generic;
using System.Data;

namespace GDMInterfaces
{
    /// <summary>
    /// Contains method to add, remove and manipulate the current data.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Removes the table with the given name.
        /// </summary>
        void DropTable(string table);
        /// <summary>
        /// Creates a new table in the dataset with a default name; Table1, Table2, etc.
        /// </summary>
        DataTable CreateTable();
        /// <summary>
        /// Adds a DataTable to the dataset.
        /// </summary>
        void AddTable(DataTable table);
        /// <summary>
        /// Returns the DataTable with given name.
        /// </summary>
        DataTable GetTable(string table);
        /// <summary>
        /// Returns a list of columns of the given table.
        /// </summary>
        List<DataColumn> GetColumns(string table);
        /// <summary>
        /// Returns a list of all tables contained in the dataset.
        /// </summary>
        List<DataTable> GetTables();
        /// <summary>
        /// The reference to the selected DataTable in the view.
        /// </summary>
        DataTable SelectedTable { get; }
        /// <summary>
        /// The name of the selected column in the view.
        /// </summary>
        string SelectedColumnName { get; }
        /// <summary>
        /// The type of the selected column in the view.
        /// </summary>
        Type SelectedColumnType{ get; }
        /// <summary>
        /// Adds a relation to the Dataset.
        /// </summary>
        void AddRelation(DataRelation relation);
        /// <summary>
        /// Exposes a reference to the Dataset currently held by the Model.
        /// </summary>
        DataSet DataSet { set; get; }

        ISet<String> GetAllTableNames();
    }

    /// <summary>
    /// Plug-ins that create output can expose a specific tag set.  These tags are placed on tables and columns by the user and stored in the data configuration. When the data configuration is executed these tags are read by the plug-in and the output is created with the tagged tables and columns as a form of input.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Identifying name of the Tag.
        /// </summary>
        public string TagName;
        /// <summary>
        /// Determines how the Tag can be placed on columns and tables.
        /// </summary>
        public TagType TagType;
        /// <summary>
        /// The list of subtags that is exposed if the current Tag is placed. 
        /// If the Tag is placed on a table the subTags are exposed to Columns and 
        /// related tables depending on the TagType of the subtags.
        /// </summary>
        public List<Tag> Subtags;
        /// <summary>
        /// The table name or the column name depending on the TagType.
        /// </summary>
        public string Entity;
    }
    /// <summary>
    /// The TagType specifies how a Tag can be placed on columns and tables.
    /// </summary>
    public enum TagType { 
        /// <summary>
        /// The tag can only be placed on tables.
        /// </summary>
        Table, 
        /// <summary>
        /// Tag can only be placed on columns of type Double.
        /// </summary>
        Double, 
        /// <summary>
        /// Tag can only be placed on columns of type Integer.
        /// </summary>
        Integer, 
        /// <summary>
        /// Tag can only be placed on columns of type Double or Integer.
        /// </summary>
        Numeric, 
        /// <summary>
        /// Tag can be placed on all column types.
        /// </summary>
        String,
        /// <summary>
        /// Tag can not be placed but indicates that it is the root of a tag tree for a specific output.
        /// </summary>
        Root }
}
