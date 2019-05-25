using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;

namespace GDMPlugins.SQL
{
    //MAde by Jon Andersson
    public  class SqlConnectDialog : UITypeEditor
    {
        public static List<string> List;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            String settings = (String)value;

            ShowDialog();
            
            return base.EditValue(context, provider, ConnectionString);
            

        }
        
          
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public SqlConnectDialog()
        {
            //ShowDialog();
            
        }

        public static String ConnectionString { get; private set; }

        public DialogResult ShowDialog()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataSource.AddStandardDataSources(dcd);
            

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                try
                {
                    // load tables
                    using (SqlConnection connection = new SqlConnection(dcd.ConnectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SELECT name FROM master..sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'ReportServer$SQLEXPRESS', 'ReportServer$SQLEXPRESSTempDB')", connection);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine(reader.HasRows);
                            }
                        }
                        connection.Close();
                        ConnectionString = dcd.ConnectionString;
                        // enable Tablename listBOx?
                    }
                    return DialogResult.OK;
                }
                catch
                {
                    MessageBox.Show("Connection failed");
                    return ShowDialog();
                }

            }
            return DialogResult.Cancel;
        }
    }
}

