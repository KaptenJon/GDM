using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DataTableToTextFileSettings : PluginSettings
    {
        public enum PredefinedCharacters { NewLine, Other, Space, Tab }
        private PredefinedCharacters _separatorCharacter;
        private string _tableName;
        private string _filePath;
        private char _otherSeparatorCharacter;
        private string _description = null;

        [DisplayName("Other Separator Character")]
        [Description("Optional, but compulsory if 'Separator Character' is set to 'Other'")]
        public char OtherSeparatorCharacter
        {
            get { return _otherSeparatorCharacter; }
            set { _otherSeparatorCharacter = value; }
        }

        [DisplayName("Separator Character")]
        [Description("Character to separate columns.")]
        public PredefinedCharacters SeparatorCharacter
        {
            get { return _separatorCharacter; }
            set { _separatorCharacter = value; }
        }

        [DisplayName("File Path")]
        [Description("The path to the file. The file will be overwritten if it already exists.")]
        [Editor(typeof(SaveFileNameEditor), typeof(UITypeEditor))]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(true)]
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        [DisplayName("Description")]
        [Description("The description of the content of the text file.")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public override async Task<bool> IsValid()
        {
           

            if (_tableName == null)
                throw new PluginException("Table name not set. ");
            if (_filePath == null)
                throw new PluginException("File path given set. ");
            if (_separatorCharacter == PredefinedCharacters.Other && _otherSeparatorCharacter == '\0')
                throw new PluginException("Separator character not set. ");

            return true;
        }

        public object GetDynamicSettings()
        {
            return new DynamicSettings(this);
        }

        private class DynamicSettings
        {
            private DataTableToTextFileSettings _settings;

            public DynamicSettings(DataTableToTextFileSettings settings)
            {
                _settings = settings;
            }

            [DisplayName("File Path")]
            [Description("The path to the file. The file will be overwritten if it already exists.")]
            [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
            public string FilePath
            {
                get { return _settings.FilePath; }
                set { _settings.FilePath = value; }
            }
        }
    }


}
