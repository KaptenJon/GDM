using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMInterfaces;

namespace GDMPlugins.SQL
{

    public class ExportSqlServer : IOutput
    {
        public string Description => "Add the data in the table to an SQL table. If primary key already exist the row is updated. Make sure that the data format is compatible";

        public string Version => "1.0";

        public string Name => "Export to SQL Server";

        public Image Icon => Icons.Database_Active_icon;


        public PluginSettings GetSettings(IModel model)
        {
            return new ExportSQLServerSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var settings = pluginSettings as ExportSQLServerSettings;
            if(settings == null)
                return;
            settings._model = model;
        }

        public Type GetSettingsType()
        {
            return typeof(ExportSQLServerSettings);
        }


        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            status.InitStatus("Export to SQL Initiated", 2);
            status.Increment();
            ExportSQLServerSettings settings = (ExportSQLServerSettings) pluginSettings;
            string connectionString = null;
            //connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=VolvoDemo;User ID=sa;Password=Chalmers";
            connectionString = settings.InitialConnString;
            //connectionString = "Data Source=localhost\\sqlexpress;User ID=sa;Password=Chalmers";
            SqlConnection myConnection = new SqlConnection(connectionString);
            try
            {
                myConnection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed");
            }
            try
            {
                SqlCommand myCommand = new SqlCommand("SET IDENTITY_INSERT "+settings.DatabaseTableName+" ON", myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch 
            {
                return;
            }
            

            string query = null;
            //query = "select * from Robot_A";


            

            var t = model.GetTable(settings.TableName);
            if(t == null)
                return;
            status.InitStatus("Export to SQL Writing Rows", t.Rows.Count);
            var schema = myConnection.GetSchema("Columns");
            var dbcolumns = (from info in schema.AsEnumerable()
                               where (string) info["Table_Name"] == settings.DatabaseTableName
                               select new
                               {
                                   TableCatalog = info["TABLE_CATALOG"],
                                   TableSchema = info["TABLE_SCHEMA"],
                                   TableName = info["TABLE_NAME"],
                                   ColumnName = info["COLUMN_NAME"],
                                   DataType = info["DATA_TYPE"]
                               }).ToArray();

            var queryStart = "INSERT INTO " + settings.DatabaseTableName +" (";

            foreach (var column in dbcolumns)
            {
                queryStart += column.ColumnName +",";
            }
            queryStart = queryStart.TrimEnd(',');
            queryStart += ") VALUES (";

            foreach (DataRow row in t.Rows)
            {
                
                query = queryStart;
                for (int index = 0; index < row.ItemArray.Length; index++)
                {
                    var column = row.ItemArray[index];
                    if ((string) dbcolumns[index].DataType == "float" || (string)dbcolumns[index].DataType == "int" || (string)dbcolumns[index].DataType == "double")
                        query += column.ToString() + ",";

                    else if (((string) dbcolumns[index].DataType).Contains("DateTime"))
                    
                        query += "'" + ((DateTime) column).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                    else
                    {
                        query += "'" + column.ToString() + "',";
                    }
                }
                query = query.TrimEnd(',');
                query += ")";
                try
                {
                    SqlCommand myCommand = new SqlCommand(query, myConnection);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    if(PluginSettings.IsInUIMode)
                        log.Add(LogType.Warning, "write data failed");
                }
                status.Increment();
            }

            try
            {
                SqlCommand myCommand = new SqlCommand("SET IDENTITY_INSERT " + settings.DatabaseTableName + " OFF", myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                return;
            }

            myConnection.Close();
            

        }


        public string GetJobDescription(PluginSettings s)
        {
            return "";
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((ExportSQLServerSettings)s).GetDynamicSettings();
        }

        public Tag Tags { get; }
    }
    public static class Extension
    {
        public static bool IsNumeric(this object s)
        {
            float output;
            return float.TryParse(s.ToString(), out output);
        }
    }
    public class ExportSQLServerSettings : PluginSettings
    {
        private string _initialConnString = null;
        private string _tableName = null;
        private string _description = null;
        internal IModel _model;
        private string _databaseTableName;
        //public enum Tables { VolvoDemo, Demo };
        //private Tables tableList;
        public ExportSQLServerSettings(IModel model)
        {
            _model = model;
        }

        public ExportSQLServerSettings()
        {
        }

        [DisplayName("Description")]
        [Description("The description of the content of the Excel file.")]
        [Browsable(false)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [Browsable(true)]
        [DisplayName("1.1 Database Connection String")]
        [Description("Please specify the database connection string!")]
        [Editor(typeof(SqlConnectDialog), typeof(UITypeEditor))]
        public string InitialConnString
        {
            get { return _initialConnString; }
            set { _initialConnString = value; }
        }


        //default, disable edit, only when connection is established, then it's possible to edit
        [DisplayName("1.2 Table name")]
        [Description("Please specify the table name")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string TableName
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _tableName;
            }
            set { _tableName = value; }
        }

        //default, disable edit, only when connection is established, then it's possible to edit
        [DisplayName("1.3 Database Table name")]
        [Description("Please specify the table name")]
        [Editor(typeof(SqlTableListBoxEditor), typeof(UITypeEditor))]
        public string DatabaseTableName
        {
            get
            {
                SqlTableListBoxEditor.InitTables(_model);
                return _databaseTableName;
            }
            set { _databaseTableName = value; }
        }

        public override async Task<bool> IsValid()
        {
            return _model.GetTable(TableName) != null;
            //string errorMsg = "";

            //if (this.initialConnString == null)
            //    errorMsg += "No database url given. ";
            //else if (this.tableName == null)
            //    errorMsg += "No table name given. ";

            //return errorMsg.Length == 0;
        }

        public object GetDynamicSettings()
        {
            return new DynamicSettings(this);
        }

        private class DynamicSettings
        {
            private ExportSQLServerSettings _settings;

            public DynamicSettings(ExportSQLServerSettings settings)
            {
                _settings = settings;
            }


        }
    }
}

