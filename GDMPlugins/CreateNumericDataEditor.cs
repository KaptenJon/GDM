using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace GDMPlugins
{
    public partial class CreateNumericDataEditor : Form
    {
        private CreateNumericDataSettings _settings;

        public CreateNumericDataEditor(CreateNumericDataSettings settings)
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
            Double result;
            if (Double.TryParse(textBox2.Text,out result))
            {
                CreateNumericDataSettings.NumericAtom atom = new CreateNumericDataSettings.NumericAtom();
                atom.Value = result;
                _settings.Expression.Add(atom);
                UpdateExpression();
            }
        }

        private void UpdateExpression()
        {
            if (_settings.Expression.Count == 0)
            {
                tabControl1.Enabled = true;
                EnableButtons(false);
                remove.Enabled = false;
                return;
            }
            else
            {
                remove.Enabled = true;
            }

            if (_settings.Expression[_settings.Expression.Count-1].Type 
                == CreateNumericDataSettings.AtomType.Operand)
            {
                tabControl1.Enabled = true;
                EnableButtons(false);
            }
            else
            {
                tabControl1.Enabled = false;
                EnableButtons(true);
            }

            expression.Text = CreateNumericDataSettings.AtomsToString(_settings.Expression);
        }

        private void EnableButtons(bool p)
        {
            minusOp.Enabled = p;
            plusOp.Enabled = p;
            divisionOp.Enabled = p;
            multiOp.Enabled = p;
        }

        // Add multi
        private void multiOp_Click(object sender, EventArgs e)
        {
            CreateNumericDataSettings.OperandAtom op = new CreateNumericDataSettings.OperandAtom();
            op.Operand = CreateNumericDataSettings.OperandType.Multiplication;
            _settings.Expression.Add(op);
            UpdateExpression();
        }

        private void minusOp_Click(object sender, EventArgs e)
        {
            CreateNumericDataSettings.OperandAtom op = new CreateNumericDataSettings.OperandAtom();
            op.Operand = CreateNumericDataSettings.OperandType.Minus;
            _settings.Expression.Add(op);
            UpdateExpression();
        }

        private void plusOp_Click(object sender, EventArgs e)
        {
            CreateNumericDataSettings.OperandAtom op = new CreateNumericDataSettings.OperandAtom();
            op.Operand = CreateNumericDataSettings.OperandType.Plus;
            _settings.Expression.Add(op);
            UpdateExpression();
        }

        private void divisionOp_Click(object sender, EventArgs e)
        {
            CreateNumericDataSettings.OperandAtom op = new CreateNumericDataSettings.OperandAtom();
            op.Operand = CreateNumericDataSettings.OperandType.Division;
            _settings.Expression.Add(op);
            UpdateExpression();
        }

        private void remove_Click(object sender, EventArgs e)
        {
            _settings.Expression.RemoveAt(_settings.Expression.Count - 1);
            UpdateExpression();
        }

        // Add column
        private void button2_Click(object sender, EventArgs e)
        {
            if (columnList.SelectedItem != null)
            {
                CreateNumericDataSettings.ColumnAtom col = new CreateNumericDataSettings.ColumnAtom();
                col.Column = (string)(columnList.SelectedItem);

                if (row.SelectedItem.Equals("Current Row"))
                {
                    col.Row = CreateNumericDataSettings.Row.CurrentRow;
                }
                else if (row.SelectedItem.Equals("Previous Row"))
                {
                    col.Row = CreateNumericDataSettings.Row.PreviousRow;
                }
                else if (row.SelectedItem.Equals("Next Row"))
                {
                    col.Row = CreateNumericDataSettings.Row.NextRow;
                }
                _settings.Expression.Add(col);
                UpdateExpression();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            columnList.Items.Clear();
            List<DataColumn> columns = _settings.Model.GetColumns(_settings.Table);

            foreach (DataColumn d in columns)
            {
                if (d.DataType == typeof(int) || d.DataType == typeof(double))
                {
                    columnList.Items.Add(d.ColumnName);
                }
            }

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
