using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace GDMPlugins
{
    public partial class DateDifferenceEditor : Form
    {
        private DateTimeDifferenceSettings _settings;

        public DateDifferenceEditor(DateTimeDifferenceSettings settings)
        {
            InitializeComponent();
            _settings = settings;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
           List<string> groupingColumns = new List<string>();

            for(int i=0; i<groupingColumnsCheckedListBox.Items.Count; i++)
            {
                if (groupingColumnsCheckedListBox.GetItemChecked(i))
                {
                    groupingColumns.Add(groupingColumnsCheckedListBox.Items[i].ToString());
                }
            }

            if (groupingColumns.Count > 0)
                _settings.GroupingColumns = groupingColumns;

            Close();
        }

        private void DateDifferenceEditor_Load(object sender, EventArgs e)
        {
            foreach (DataTable table in _settings.Model.GetTables())
            {
                tableComboBox.Items.Add(table.TableName);
            }

            if (tableComboBox.Items.Count > 0) tableComboBox.SelectedIndex = 0;
        }

        private void tableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableComboBox.SelectedItem.ToString() == "")
                return;

            if (groupingColumnsCheckedListBox.Items.Count > 0)
                groupingColumnsCheckedListBox.Items.Clear();

            foreach (DataColumn column in _settings.Model.GetTable(tableComboBox.SelectedItem.ToString()).Columns)
            {
                groupingColumnsCheckedListBox.Items.Add(column.ColumnName);
            }
        }
    }
}
