using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using GDMInterfaces;

namespace GDMPlugins
{
    public class TextFileSettings : PluginSettings
    {
        private string _fileName = null;
        // Only used when rowEndCharSelection is Other
        private char _ownEndChar;
        private RowEndChar _rowEndCharSelection;
        private string _description = null;
        public enum RowEndChar {NewLine, Other, Tab, Space};

        [DisplayName("Row End Character")]
        [Description("Choose \"Other\" to define a new row end character not listed.")]
        public RowEndChar RowEndCharSelection
        {
            get { return _rowEndCharSelection; }
            set { _rowEndCharSelection = value; }
        }

        [DisplayName("File Path")]
        [Description("Path to the source.")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        [DisplayName("Description")]
        [Description("The description of the content of the text file.")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DisplayName("Other Row End character")]
        [Description("Define a new row end character that is not predefined.")]
        public char OwnEndChar
        {
            get { return _ownEndChar; }
            set { _ownEndChar = value; }
        }

        public override async Task<bool> IsValid()
        {
            

            if (_fileName == null)
                throw new PluginException("No filename given. ");
            else if (!File.Exists(_fileName))
                throw new PluginException("File does not exist. ");
            if (_rowEndCharSelection == RowEndChar.Other && _ownEndChar == '\0')
                throw new PluginException("No row end character defined. ");

            return true;
        }

        public object GetDynamicSettings()
        {
            return new DynamicSettings(this);
        }

        private class DynamicSettings
        {
            private TextFileSettings _settings;

            public DynamicSettings(TextFileSettings settings)
            {
                _settings = settings;
            }

            [DisplayName("Filename")]
            [Description("Path to the source.")]
            [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
            public string FileName
            {
                get { return _settings.FileName; }
                set { _settings.FileName = value; }
            }
        }
    }
}
