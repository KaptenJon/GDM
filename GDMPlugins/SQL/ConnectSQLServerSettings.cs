using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins.SQL
{
    public class ConnectSqlServerSettings : PluginSettings
    {
        private string _initialConnString = null;
        private string _tableName = null;
        private string _description = null;
        private IModel _model;
        //public enum Tables { VolvoDemo, Demo };
        //private Tables tableList;
        public ConnectSqlServerSettings(IModel model)
        {
            _model = model;
        }

        public ConnectSqlServerSettings()
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
        [Description("Ignored if select is filled. Please specify the table name")]
        [Editor(typeof(SqlTableListBoxEditor), typeof(UITypeEditor))]
        public string TableName
        {
            get {
                SqlTableListBoxEditor.InitTables(_model);
                return _tableName;
            }
            set { _tableName = value; }
        }

        [Description("If this is enterd all other following properties are ignored")]
        [DisplayName("2 Optionally select statement")]
        public string Select { get; set; }

        [Description("Ignored if select is filled. Optionally Top clausule added into the select only for ms sql use Custom full Select for other databases (eg.TOP 100). Combine with order by")]
        [DisplayName("3.1 Optionally Top statement")]
        public int Top { get; set; }
        [DisplayName("3.2 Optionally Orderby statement")]
        [Description("Ignored if select is filled. Optionally orderby clausule added into the select")]
        public string OrderBy { get; set; }

        [DisplayName("3.3 Optionally where statement")]
        [Description("Ignored if select is filled. Optionally where clausule added into the select. To add current datetime use e.g {Datetime yyyy-MM-dd} or for historic time {Datetime,-5 days,yyyy-MM-dd} or {Datetime,-5 hours,yyyy-MM-dd hh-mm-ss}")]
        public string Where { get; set; }

        public override async Task<bool> IsValid()
        {
            return true;
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
            private ConnectSqlServerSettings _settings;

            public DynamicSettings(ConnectSqlServerSettings settings)
            {
                _settings = settings;
            }

            
        }
    }
}

