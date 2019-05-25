using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Accord.Math;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using GDMInterfaces;

namespace GDMPlugins.SQL
{
    public class ConnectSqlServer : IInput
    {
        public string Description => "Read a table/view from sql server and display the data into a DataTable";

        public string Version => "1.0";

        public string Name => "Connect SQL Server";

        public Image Icon =>  Icons.Database_Active_icon;

        public bool ShowSettingsObject
        {
            get { throw new NotImplementedException(); }
        }

        public PluginSettings GetSettings(IModel model)
        {
            return new ConnectSqlServerSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            
        }

        public Type GetSettingsType()
        {
            return typeof(ConnectSqlServerSettings);
        }


        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            status.InitStatus("SQL Connect" , 2);
            status.Increment();
            ConnectSqlServerSettings settings = (ConnectSqlServerSettings)pluginSettings;
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
            var table = model.CreateTable();
            string query = null;
            //query = "select * from Robot_A";
            if (String.IsNullOrWhiteSpace(settings.Select))
            {
                query = "select * from " + settings.TableName;
                if (settings.Top >0)
                    query = "select Top"+settings.Top+" * from " + settings.TableName;
                if (!String.IsNullOrWhiteSpace(settings.OrderBy))
                    query += (settings.OrderBy.ToLowerInvariant().StartsWith("order by")?" ":"order by") + settings.OrderBy.ToLowerInvariant();
                
                if (!String.IsNullOrWhiteSpace(settings.Where))
                {
                    var where = settings.Where;
                    var datetime = where.Split('{', '}');
                    for (int i = 0; i < datetime.Length-1; i++)
                    {

                        if (datetime[i].ToLowerInvariant().StartsWith("datetime"))
                        {
                            var dateparse = datetime[i].Split(',');
                            if (dateparse.Length == 1)
                                datetime[i] = '\'' + DateTime.Now.ToShortTimeString() + '\'';
                            else if(dateparse.Length ==2)
                            {
                                datetime[i] = '\''+ DateTime.Now.ToString(dateparse[1])+ '\'';
                            }
                            else if (dateparse.Length == 3)
                            {
                                var subtract = dateparse[1].Split(' ');
                                double number = 0;
                                double.TryParse(subtract[0], out number);
                                TimeSpan minus = TimeSpan.Zero;
                                if (subtract[1].ToLowerInvariant().Contains("day"))
                                    minus = TimeSpan.FromDays(number);
                                else if (subtract[1].ToLowerInvariant().Contains("hour"))
                                    minus = TimeSpan.FromHours(number);
                                else if (subtract[1].ToLowerInvariant().Contains("year"))
                                    minus = TimeSpan.FromDays(365*number);
                                else if (subtract[1].ToLowerInvariant().Contains("minute"))
                                    minus = TimeSpan.FromMinutes(number);
                                else if (subtract[1].ToLowerInvariant().Contains("month"))
                                    minus = TimeSpan.FromDays(number*30);
                                else if (subtract[1].ToLowerInvariant().Contains("second"))
                                    minus = TimeSpan.FromSeconds(number);
                               
                                datetime[i] = '\''+ (DateTime.Now + minus).ToString(dateparse[2])+ '\'';
                            }
                            i++;
                        }

                    }
                    where = "";
                    foreach (var s in datetime)
                    {
                        where += s;
                    }
                     
                    
                    query += (where.ToLowerInvariant().StartsWith("where") ? " " : " Where ") + where;
                }
            }
            else
            {
                query = settings.Select;
            }
            //query = "SELECT name FROM master..sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'ReportServer$SQLEXPRESS', 'ReportServer$SQLEXPRESSTempDB')";
            try
            {
                SqlCommand myCommand = new SqlCommand(query, myConnection);
                table.Load(myCommand.ExecuteReader());
            }
            catch (Exception)
            {
                MessageBox.Show("Read data failed");
            }

            myConnection.Close();
            status.Increment();
        }


        public string GetJobDescription(PluginSettings s)
        {
            return "";
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((ConnectSqlServerSettings)s).GetDynamicSettings();
        }
    }
}


