using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GDMInterfaces;

namespace GDMPlugins.Excel
{
    public class ExcelSettings : PluginSettings
    {
        
        public enum Sheet { All, Specific };
        private Sheet _sheetOption;
        private string _fileName = null;
        private string _sheetName = null;    // Only used if sheetOption is set to Specific
        private string _description = null;
        private bool _hdr =true;



        public ExcelSettings()
        {
            
        }

        public void ShowDialog()
        {
            if(!IsInUIMode || !String.IsNullOrEmpty(_fileName))
                return;
            var file = new OpenFileDialog();
            file.DefaultExt = "xlsx";
            file.Filter = "(*.xls, *.xlsx)|*.xls;*.xlsx";
            if (file.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                _fileName = file.FileName;
            }

        }

        [DisplayName("Header Row")]
        [Description("Given that the row in question is of datatype string the following options are available. True: use first row as column names. False: treat first row as any other data row.")]
        public bool Hdr
        {
            get { return _hdr; }
            set { _hdr = value; }
        }



        [DisplayName("Sheets")]
        [Description("All: all sheets will be converted into a corresponding datatable. Specific: one, specified sheet")]
        public Sheet SheetOption
        {
            get { return _sheetOption; }
            set { _sheetOption = value; }
        }

        [DisplayName("Sheet Name")]
        [Description("The name of the sheet to extract from the Excel file.")]
        
        public string SheetName
        {
            get
            {
               
                return _sheetName;
            }
            set { _sheetName = value; }
        }




        [DisplayName("File Path")]
        [Description("Path to the source.")]
        [Editor(typeof(ExcelFileNameEditor), typeof(UITypeEditor))]
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
            else if (!(File.Exists(_fileName) || URLExists(_fileName)))
                errorMsg += "The file does not exist. ";
            if (_sheetOption==Sheet.Specific && _sheetName == null)
                errorMsg += "No sheet name given. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return errorMsg.Length == 0;
        }
        public bool URLExists(string url)
        {
            bool result = true;

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 2000; // miliseconds
                webRequest.Method = "HEAD";

                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }
        public object GetDynamicSettings() 
        {
            return new DynamicSettings(this);
        }

        private class DynamicSettings
        {
            private ExcelSettings _settings;

            public DynamicSettings(ExcelSettings settings)
            {
                _settings = settings;
            }

            [DisplayName("Filename")]
            [Description("Path to the source.")]
            [Editor(typeof(ExcelFileNameEditor), typeof(UITypeEditor))]
            public string FileName
            {
                get { return _settings.FileName; }
                set  { _settings.FileName = value; }
            }
        }

        
    }
    public class ExcelFileNameEditor : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            
            base.InitializeDialog(openFileDialog);
            openFileDialog.Filter = "(*.xls, *.xlsx)|*.xls;*.xlsx";
            openFileDialog.DefaultExt = "xlsx";
            openFileDialog.Multiselect = false;
            openFileDialog.FileOk += OpenFileDialog_FileOk;
            
            
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var openfile = sender as OpenFileDialog;
            EditValue(null, openfile.FileName);
            PluginSettings.LockPlugin = DateTime.Now;
            
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            return base.EditValue(context, provider, value);
           
        }
    }
}

