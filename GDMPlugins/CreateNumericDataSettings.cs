using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CreateNumericDataSettings : PluginSettings
    {
        private string _table;
        private string _columnname;
        private List<Atom> _atoms = new List<Atom>();
        private IModel _model;

        public CreateNumericDataSettings(){}

        public CreateNumericDataSettings(IModel model)
        {
            _model = model;
            
        }

        [DisplayName("Settings")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(CreateDataEditor), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public CreateNumericDataSettings Settings
        {
            get { return this; }
            set {  }
        }

        [XmlIgnore]
        [Browsable(false)]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [Browsable(false)]
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        [Browsable(false)]
        public string Columnname
        {
            get { return _columnname; }
            set { _columnname = value; }
        }

        [Browsable(false)]
        public List<Atom> Expression
        {
            get { return _atoms; }
            set { _atoms = value; }
        }

        public override async Task<bool> IsValid()
        {

            if (_table.Length > 0 && _columnname.Length > 0 && _atoms.Count > 0)
            {
                
                return true;
            }
            else
            {
                throw new PluginException("Unknown error occured");
            }
        }

        public override string ToString()
        {
            return AtomsToString(Expression);
        }

        public static string AtomsToString(List<Atom> list) 
        {
            string expression = "";

            foreach (Atom a in list)
            {
                switch (a.Type)
                {
                    case AtomType.Operand:
                        switch (((OperandAtom)a).Operand)
                        {
                            case OperandType.Plus:
                                expression += " + ";
                                break;
                            case OperandType.Multiplication:
                                expression += " * ";
                                break;
                            case OperandType.Division:
                                expression += " / ";
                                break;
                            case OperandType.Minus:
                                expression += " - ";
                                break;
                        }
                        break;
                    case AtomType.Numeric:
                        expression += ((NumericAtom)a).Value.ToString();
                        break;
                    case AtomType.Column:
                        expression += ((ColumnAtom)a).Row.ToString()
                            + "(" + ((ColumnAtom)a).Column + ")";
                        break;
                    case AtomType.Function:
                        expression += ((FunctionAtom)a).Function.ToString()
                            + "(" + ((FunctionAtom)a).Key + ","
                            + ((FunctionAtom)a).Table + ","
                            + ((FunctionAtom)a).Column + ")";
                        break;
                }
            }
            return expression;
        }


        public enum AtomType { Operand, Numeric, Column, Function }
        public enum Row { PreviousRow, CurrentRow, NextRow }
        public enum FunctionType { Sum, Count}
        public enum OperandType { Plus, Minus, Multiplication, Division}

        [XmlInclude(typeof(FunctionAtom))]
        [XmlInclude(typeof(OperandAtom))]
        [XmlInclude(typeof(NumericAtom))]
        [XmlInclude(typeof(ColumnAtom))]
        public class Atom
        {
            [XmlAttribute("type")]
            public AtomType Type;
        }

        public class OperandAtom : Atom
        {
            public OperandAtom() { Type = AtomType.Operand; }
            [XmlAttribute("operand")]
            public OperandType Operand;
        }

        public class NumericAtom : Atom
        {
            public NumericAtom() { Type = AtomType.Numeric; }
            [XmlAttribute("value")]
            public double Value;
        }

        public class ColumnAtom : Atom
        {
            public ColumnAtom() { Type = AtomType.Column; }
            [XmlAttribute("row")]
            public Row Row;
            [XmlAttribute("column")]
            public string Column;
        }

        public class FunctionAtom : Atom
        {
            public FunctionAtom() { Type = AtomType.Function; }
            [XmlAttribute("function")]
            public FunctionType Function;
            [XmlAttribute("key")]
            public string Key;
            [XmlAttribute("table")]
            public string Table;
            [XmlAttribute("column")]
            public string Column;
        }

        private class CreateDataEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                CreateNumericDataSettings settings = (CreateNumericDataSettings)value;
                CreateNumericDataEditor editor = new CreateNumericDataEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, null);
            }
        }
    }
}

