using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GDMInterfaces
{
    public class SelectManyEditorDropdown : UITypeEditor
    {
        //this is a container for strings, which can be picked-out
        CheckedListBox _box1 = new CheckedListBox();
        IWindowsFormsEditorService _edSvc;
        //this is a string array for drop-down list
        public static List<string> List;

        public static string CurrentConstant { get; set; }



        public SelectManyEditorDropdown()
        {
            _box1.BorderStyle = BorderStyle.None;
            _box1.ItemCheck += _box1_ItemCheck;
            //add event handler for drop-down box when item will be selected

        }

        private bool stoprec = false;
        private void _box1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(stoprec)
                return;
            if( _box1?.Items[e.Index] as string == "All" )
                for (int index = 0; index < _box1.Items.Count; index++)
                {
                    if (index != e.Index)
                        _box1.SetItemChecked(index, e.NewValue == CheckState.Checked);
                }
            else if(e.NewValue == CheckState.Unchecked && _box1.Items.Contains("All"))
            {
                stoprec = true;
                _box1.SetItemChecked(_box1.Items.IndexOf("All"), false);
                stoprec = false;


            }
                
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _box1.Items.Clear();
            foreach (string s in List) _box1.Items.Add(s);
            
                

            var oldlist = value as string[];
            if (oldlist != null)
            {
                
                for (int index = 0; index < _box1.Items.Count; index++)
                {
                    if(oldlist.Contains(_box1.Items[index]))
                        _box1.SetItemChecked(index, true);
                }
            }
            _box1.Height = _box1.PreferredHeight;
            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _edSvc.DropDownControl(_box1);
                return _box1.CheckedItems.Cast<String>().ToArray();

            }
            return value;
        }



        public static void ColumnsInTable(IModel model, string table)
        {
            List<string> columns = new List<string>();
            if (model == null) return;
            foreach (DataColumn c in model.GetColumns(table))
            {
                columns.Add(c.ColumnName);
            }
            List = columns;
        }

        public static void ColumnsInTable(IModel model, string table, Type columntype)
        {
            List<string> columns = new List<string>();
            if (model == null) return;
            foreach (DataColumn c in model.GetColumns(table))
            {
                if (c.DataType == columntype) columns.Add(c.ColumnName);
            }
            List = columns;
        }

        public static void Tables(IModel model)
        {
            List<string> tables = new List<string>();
            if (model == null) return;
            foreach (DataTable d in model.GetTables())
            {
                tables.Add(d.TableName);
            }
            if (CurrentConstant != null)
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


    }
}
