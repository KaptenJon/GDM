using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CreateNumericData : ITool
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Create Data";

        public string Description => "Create a new data column in a table by combining other numeric column values and simple arithmetic.";

        public string Version => "1.0";

        public string Name => "Create Numeric Data Column";

        public Image Icon => Icons.CreateData;

        public PluginSettings GetSettings(IModel model)
        {
            return new CreateNumericDataSettings(model);
        }

        public Type GetSettingsType()
        {
            return typeof(CreateNumericDataSettings);
        }

        private double CalculateExpression(DataRowCollection rows, List<CreateNumericDataSettings.Atom> exp, int rowindex)
        {
            if (exp.Count == 0) return 0.0;
            int atomindex = 0;
            double value = CalculateValue(exp[atomindex], rows, rowindex);
            atomindex++;

            while (atomindex < exp.Count)
            {
                if (atomindex + 1 < exp.Count)
                {
                    double nextValue = CalculateValue(exp[atomindex+1], rows, rowindex);
                    CreateNumericDataSettings.OperandAtom a = exp[atomindex] as CreateNumericDataSettings.OperandAtom;
                    if (a != null)
                    {
                        switch (a.Operand)
                        {
                            case CreateNumericDataSettings.OperandType.Division:
                                value /= nextValue; break;
                            case CreateNumericDataSettings.OperandType.Minus:
                                value -= nextValue; break;
                            case CreateNumericDataSettings.OperandType.Multiplication:
                                value *= nextValue; break;
                            case CreateNumericDataSettings.OperandType.Plus:
                                value += nextValue; break;
                        }
                    }
                }
                atomindex += 2;
            }
            return value;
        }

        private double CalculateValue(CreateNumericDataSettings.Atom atom, DataRowCollection rows, int rowindex)
        {
            if (atom.Type == CreateNumericDataSettings.AtomType.Numeric)
            {
                return ((CreateNumericDataSettings.NumericAtom)atom).Value;
            }
            else if (atom.Type == CreateNumericDataSettings.AtomType.Column)
            {
                CreateNumericDataSettings.ColumnAtom c = (CreateNumericDataSettings.ColumnAtom)atom;
                switch (c.Row)
                {
                    case CreateNumericDataSettings.Row.CurrentRow:
                        return Convert.ToDouble(rows[rowindex][c.Column]);
                    case CreateNumericDataSettings.Row.PreviousRow:
                        if (rowindex - 1 >= 0) return Convert.ToDouble(rows[rowindex - 1][c.Column]);
                        else return 0;
                    case CreateNumericDataSettings.Row.NextRow:
                        if (rowindex + 1 < rows.Count) return Convert.ToDouble(rows[rowindex + 1][c.Column]);
                        else return 0;
                }
            }
            else if (atom.Type == CreateNumericDataSettings.AtomType.Function)
            {
                throw new NotImplementedException("Not yet implemented");
            }
            throw new Exception("Illegal expression");
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            CreateNumericDataSettings settings = (CreateNumericDataSettings)pluginSettings;
            DataTable table = model.GetTable(settings.Table);
            status.InitStatus("Creating column...", table.Rows.Count);

            table.Columns.Add(settings.Columnname, typeof(double));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                double result = CalculateExpression(table.Rows, settings.Expression, i);
                table.Rows[i][settings.Columnname] = result;
                status.Increment();
            }
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            return;
        }
    }
}

