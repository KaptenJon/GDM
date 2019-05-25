using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GDMInterfaces;

namespace GDMPlugins.SQL
{ 
    class SqlTableListBoxEditor : UITypeEditor
    {
        //this is a container for strings, which can be picked-out
        ListBox _box1 = new ListBox();
        IWindowsFormsEditorService _edSvc;
        //this is a string array for drop-down list
        public static List<string> List;
         

        public SqlTableListBoxEditor()
        {
            _box1.BorderStyle = BorderStyle.None;
            //add event handler for drop-down box when item will be selected
            _box1.Click += Box1_Click;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _box1.Items.Clear();
            if (List == null)
                return null;
            foreach (string s in List) _box1.Items.Add(s);
            _box1.Height = _box1.PreferredHeight;
            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _edSvc.DropDownControl(_box1);
                return _box1.SelectedItem;

            }
            return value;
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            _edSvc.CloseDropDown();
        }

        internal static void Tables()
        {
            List<string> tables = new List<string>();

            string connString = "";
            connString = SqlConnectDialog.ConnectionString;
            SqlCommand command;
            string sql = "SELECT * FROM sys.tables";
            SqlDataReader dataReader;
            if (connString != null)
            {      
                SqlConnection cnn;                  
                cnn = new SqlConnection(connString);
                try
                {
                    cnn.Open();
                    //MessageBox.Show("Connection Open ! ");                   
                    command = new SqlCommand(sql, cnn);
                    dataReader = command.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        tables.Add(dataReader.GetString(0));
                        //MessageBox.Show(dataReader.GetString(0));
                    }
                    dataReader.Close();
                    command.Dispose();
                    cnn.Close();
                    List = tables;
                }               
                catch
                {
                    MessageBox.Show("Read table list failed!");
                }
            }

        }


        public static void InitTables(IModel model)
        {
            Tables();

        }

    }

}
