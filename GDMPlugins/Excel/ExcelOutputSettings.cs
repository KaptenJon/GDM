using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMInterfaces;

namespace GDMPlugins.Excel
{
    public class ExcelOutputSettings : PluginSettings
    {
        
        public enum Sheet { All, Specific };

        private string _fileName = null;
        private string _sheetName = null;    // Only used if sheetOption is set to Specific
        private string _description = null;

        private string _decimal = "System Default";

        public ExcelOutputSettings()
        {
            
        }

        public void ShowDialog()
        {
            if(!IsInUIMode || !String.IsNullOrEmpty(_fileName))
                return;
            var file = new SaveFileDialog();
            file.DefaultExt = "xlsx";
            file.Filter = "(*.xls, *.xlsx)|*.xls;*.xlsx";
            if (file.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                _fileName = file.FileName;
            }

        }
        [DisplayName("Decimal seperator")]
        [Description("The name of the sheet to extract from the Excel file.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string Decimal
        {
            get
            {
                ListBoxEditor.List = new List<object>(new[] {"System Default", ". (dot)", ", (comma)" });
                return _decimal;
            }
            set { _decimal = value; }
        }
        [DisplayName("Table Name")]
        [Description("The name of the sheet to extract from the Excel file.")]
        public string Table
        {
            get { return _sheetName; }
            set { _sheetName = value; }
        }

        [DisplayName("File Path")]
        [Description("Path to the source.")]
        [Editor(typeof(SaveFileNameEditor), typeof(UITypeEditor))]
        [ReadOnly(false)]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                
                 
            }
        }

        [DisplayName("Description")]
        [Description("The description of the content of the Excel file.")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public override async Task<bool> IsValid()
        {
            
            var errorMsg = "";

            if (_fileName == null)
                errorMsg += "No filename given. ";
            else if (_sheetName == null)
                errorMsg += "No Table name given. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return errorMsg.Length == 0;
        }
  

       
    }
    
}

