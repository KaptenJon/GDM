using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CreateStringDataSettings : PluginSettings
    {
        private string _table;
        private string _columnname;
        private IModel _model;
        private List<StringBase> _stringlist = new List<StringBase>();

        public CreateStringDataSettings(){}

        public CreateStringDataSettings(IModel model)
        {
            _model = model;
        }

        [DisplayName("Settings")]
        [Description("Click the button to bring forth the settings panel.")]
        [Editor(typeof(CreateStringDataEditor_), typeof(UITypeEditor))]
        [ReadOnly(true)]
        [XmlIgnore]
        public CreateStringDataSettings Settings
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
        public List<StringBase> StringList
        {
            get { return _stringlist; }
            set { _stringlist = value; }
        }

        public override async Task<bool> IsValid()
        {
           

            if (_table == null)
                throw new PluginException("Table not specified. ");
            else if (_columnname == null)
                throw new PluginException("Column name not specified. ");
            else if (_stringlist.Count <= 0)
                throw new PluginException("No expression specified. ");

            return true;
        }

        public override string ToString()
        {
            return StringListToString(StringList);
        }

        public static string StringListToString(List<StringBase> list) 
        {
            StringBuilder expression = new StringBuilder();

            foreach (StringBase a in list)
            {
                if (expression.Length > 0)  expression.Append(" & ");
                ColumnValue col = a as ColumnValue;
                if (col != null) 
                {
                  expression.Append(col.Row.ToString() + "(" + col.Column + ")");
                }
                else 
                {
                    expression.Append(((StringValue)a).String);
                }
            }
            return expression.ToString();
        }

        public enum RowType { PreviousRow, CurrentRow, NextRow }

        [XmlInclude(typeof(StringValue))]
        [XmlInclude(typeof(ColumnValue))]
        public class StringBase { }
        public class StringValue : StringBase
        {
            [XmlAttribute("string")]
            public string String;
        }
        public class ColumnValue : StringBase
        {
            [XmlAttribute("row")]
            public RowType Row;
            [XmlAttribute("column")]
            public string Column;
        }
        

        private class CreateStringDataEditor_ : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                CreateStringDataSettings settings = (CreateStringDataSettings)value;
                CreateStringDataEditor editor = new CreateStringDataEditor(settings);
                editor.ShowDialog();
                return base.EditValue(context, provider, null);
            }
        }
    }
}

