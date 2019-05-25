using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GDMInterfaces;

namespace GDMInterfaces
{
    public class ListBoxEditor : UITypeEditor
    {
        //this is a container for strings, which can be picked-out
        ListBox _box1 = new ListBox();
        IWindowsFormsEditorService _edSvc;
        //this is a string array for drop-down list
        public static List<object> List;


        public static string CurrentConstant { get; set; }
        public ListBoxEditor()
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
            foreach (object s in List) _box1.Items.Add(s);
            _box1.Height = _box1.PreferredHeight;
            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _edSvc.DropDownControl(_box1);
                return _box1.SelectedItem;

            }
            return  value;
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            _edSvc.CloseDropDown();
        }

        public static void ColumnsInTable(IModel model, string table)
        {
            List<object> columns = new List<object>();
            if (model == null) return;
            foreach (DataColumn c in model.GetColumns(table))
            {
                columns.Add(c.ColumnName);
            }
            List = columns;
        }

        public static void ColumnsInTable(IModel model, string table, Type columntype)
        {
            List<object> columns = new List<object>();
            if (model == null) return;
            foreach (DataColumn c in model.GetColumns(table))
            {
                if (c.DataType == columntype) columns.Add(c.ColumnName);
            }
            List = columns;
        }

        public static void Tables(IModel model)
        {
            List<object> tables = new List<object>();
            if (model == null) return;
            foreach (DataTable d in model.GetTables())
            {
                tables.Add(d.TableName);
            }
            if(CurrentConstant != null)
                tables.Add(CurrentConstant);
            List = tables;
        }

        public static bool IsColumn(string column, string table, IModel model)
        {
            foreach (DataColumn d in model.GetColumns(table))
            {
                if (column == d.ColumnName) return true;
            }
            return false;
        }

        public static bool IsTable(string table, IModel model)
        {
            foreach (DataTable d in model.GetTables())
            {
                if (table == d.TableName) return true;
            }
            return false;
        }

        public static void SetColumns(IModel model, string value, ref string var)
        {
            if (model == null)
            {
                var = value;
            }
            else
            {
                if (IsColumn(value, model.SelectedTable.TableName, model))
                {
                    var = value;
                }
            }
        }

        public static string GetColumns(IModel model, string var)
        {
            if (model != null) ColumnsInTable(model, model.SelectedTable.TableName);
            return var;
        }

        public static void InitTables(IModel model)
        {
            Tables(model);
            
        }

        public static void InitColumns(IModel model, string dataSourceTable)
        {
            ColumnsInTable(model, dataSourceTable);
        }

        public static void InitColumns(IModel model, string dataSourceTable, Type type)
        {
            ColumnsInTable(model, dataSourceTable, type);
                
        }


        public static void InitUniqeRowEntries(IModel model, string table, string sequenzeColumn)
        {
            if(model == null || table==null||sequenzeColumn==null)
                return;
            DataTable datatable = model.GetTable(table);
            if(datatable==null || !datatable.Columns.Contains(sequenzeColumn))
                return;
  
            var d = (from p in datatable.AsEnumerable() select p[sequenzeColumn]).Distinct();
            List = d.ToList();
        }
    }
}
