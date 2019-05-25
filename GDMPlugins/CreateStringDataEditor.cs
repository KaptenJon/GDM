using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace GDMPlugins
{
    public partial class CreateStringDataEditor : Form
    {
        private CreateStringDataSettings _settings;

        public CreateStringDataEditor(CreateStringDataSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            textBox1.Text = _settings.Columnname;
            row.SelectedIndex = 0;
            string tableSelected;

            if (settings.Table != null)
            {
                tableSelected = settings.Table;
            }
            else
            {
                tableSelected = _settings.Model.SelectedTable.TableName;
            }

            foreach (DataTable table in _settings.Model.GetTables())
            {
                string tableName = table.TableName;
                comboBox1.Items.Add(tableName);
                if (tableName == tableSelected)
                {
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                }
            }
            UpdateExpression();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        // Numeric add
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                CreateStringDataSettings.StringValue s = new CreateStringDataSettings.StringValue();
                s.String = textBox2.Text;
                _settings.StringList.Add(s);
                UpdateExpression();
            }
        }

        private void UpdateExpression()
        {
            if (_settings.StringList.Count == 0)
            {
                tabControl1.Enabled = true;
                remove.Enabled = false;
            }
            else
            {
                remove.Enabled = true;
            }

            expression.Text = CreateStringDataSettings.StringListToString(_settings.StringList);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            _settings.StringList.RemoveAt(_settings.StringList.Count - 1);
            UpdateExpression();
        }

        // Add column
        private void button2_Click(object sender, EventArgs e)
        {
            if (columnList.SelectedItem != null)
            {
                CreateStringDataSettings.ColumnValue col = new CreateStringDataSettings.ColumnValue();
                col.Column = (string)(columnList.SelectedItem);

                if (row.SelectedItem.Equals("Current Row"))
                {
                    col.Row = CreateStringDataSettings.RowType.CurrentRow;
                }
                else if (row.SelectedItem.Equals("Previous Row"))
                {
                    col.Row = CreateStringDataSettings.RowType.PreviousRow;
                }
                else if (row.SelectedItem.Equals("Next Row"))
                {
                    col.Row = CreateStringDataSettings.RowType.NextRow;
                }
                _settings.StringList.Add(col);
                UpdateExpression();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            columnList.Items.Clear();
            List<DataColumn> columns = _settings.Model.GetColumns(_settings.Table);

            foreach (DataColumn d in columns)
                columnList.Items.Add(d.ColumnName);

            if (columnList.Items.Count > 0)
            {
                if (columnList.SelectedItem == null)
                    columnList.SelectedItem = columnList.Items[0];
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.Table = (string)comboBox1.SelectedItem;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _settings.Columnname = textBox1.Text;
        }
    }
}
