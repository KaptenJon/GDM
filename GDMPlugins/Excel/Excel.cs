using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel;
using GDMInterfaces;

namespace GDMPlugins.Excel
{
    
    public class Excel : IInput
    {
       

        public Excel()
        {
            
        }
        public string Description => "Turns an Excel file into a DateTable";

        public string Version => "1.0";

        public string Name => "Excel File";

        public Image Icon => Icons.Excel;

        

        public PluginSettings GetSettings(IModel model)
        {
            
            var e = new ExcelSettings();
            
            return e;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var setting = pluginSettings as ExcelSettings;
            if(setting == null)
                return;
            if(ListBoxEditor.CurrentConstant != null)
                setting.FileName = ListBoxEditor.CurrentConstant;
            //setting.ShowDialog();

        }

        

        public Type GetSettingsType()
        {
            return typeof(ExcelSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var set = pluginSettings as ExcelSettings;
            if (set == null) return;
            DataSet result = null;
            status.InitStatus("Loading " + set.FileName, 3 );
            status.Increment();
            try
            {
                try
                {
                    IExcelDataReader excelReader =
                        ExcelReaderFactory.CreateOpenXmlReader(new FileInfo(set.FileName).OpenRead());
                    excelReader.IsFirstRowAsColumnNames = set.Hdr;
                    result = excelReader.AsDataSet();
                    if (result == null)
                        throw new Exception();
                        
                    
                    status.Increment();
                    excelReader.Close();
                }
                catch (IOException)
                {
                    log.Add(LogType.Error, "Close the file " + set.FileName);
                    status.InitStatus("",0);
                    throw;
                }
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw;
                try
                {
                    IExcelDataReader excelReader =
                        ExcelReaderFactory.CreateBinaryReader(new FileInfo(set.FileName).OpenRead());
                    excelReader.IsFirstRowAsColumnNames = set.Hdr;
                    result = excelReader.AsDataSet();
                    status.Increment();
                    excelReader.Close();
                }
                catch (IOException)
                {
                    log.Add(LogType.Error, "Close the file " + set.FileName);
                    status.InitStatus("", 0);
                    throw;
                }
            }
            if (result == null)
                return;
            var tabels = result.Tables;
            if (set.SheetOption == ExcelSettings.Sheet.All)
            {
                foreach (DataTable t in tabels)
                {
                    model.AddTable(t.Copy());
                }
            }
            else
                foreach (DataTable t in tabels)
                {
                    if (t.TableName == set.SheetName)
                        model.AddTable(t.Copy());
                }

            status.Increment();
            
            //ApplyNPIO(model, pluginSettings, log, status);
            //ApplyOle(model, pluginSettings, log, status);

        }

        //public void ApplyNPIO(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        //{
        //    var set = pluginSettings as ExcelSettings;
        //    if (set == null) return;
        //    var exs = new ExcelService();
        //    var wb = exs.ReadWorkbook(set.FileName);
        //    var tabels = wb.AsDataSet(set.Hdr).Tables;
            

        //    if (set.SheetOption == ExcelSettings.Sheet.All)
        //    {
        //        foreach (DataTable t in tabels)
        //        {
        //            model.AddTable(t.Copy());
        //        }
        //    }
        //    else
        //        foreach (DataTable t in tabels)
        //        {
        //            if(t.TableName == set.SheetName)
        //                model.AddTable(t.Copy());
        //        }
            
        //}
     

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="pluginSettings"></param>
        //private void ApplyEPPlus(IModel model, PluginSettings pluginSettings)
        //{
        //    var set = pluginSettings as ExcelSettings;
        //    if (set == null) return;
        //    using (ExcelPackage pck = new ExcelPackage(new FileInfo(set.FileName)))
        //    {
        //        if (set.SheetOption == ExcelSettings.Sheet.All)
        //        {
        //            foreach (ExcelWorksheet ws in pck.Workbook.Worksheets)
        //            {
        //                ImportWorksheet(model, ws, set);
        //            }
        //        }
        //        else
        //        {
        //            var ws = pck.Workbook.Worksheets[set.SheetName];
        //            ImportWorksheet(model, ws, set);
        //        }
        //    }
        //}

        //private void ImportWorksheet(IModel model, ExcelWorksheet ws, ExcelSettings set)
        //{
        //    DataTable table = model.CreateTable();
        //    if (model.GetTable(ws.Name) == null)
        //        table.TableName = ws.Name;


        //    for (var i = 1; i <= ws.Dimension.End.Column; i++)
        //    {
        //        var firstRowCell = ws.Cells[set.HdrRowNr, 1, set.HdrRowNr, ws.Dimension.End.Column][1, i];
        //        var colname = set.Hdr
        //            ? firstRowCell.Text
        //            : string.Format("Column {0}", firstRowCell.Start.Column);

        //        table.Columns.Add(colname, GetColumnType(ws.Cells[set.HdrRowNr + 1, i, ws.Dimension.End.Row, i].AsEnumerable()));
        //    }

        //    for (int rowNum = set.HdrRowNr + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
        //    {
        //        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        //        DataRow row = table.Rows.Add();
        //        int c = 0;

        //        foreach (var cell in wsRow)
        //        {
        //            if (table.Columns[c++].DataType == typeof (string))
        //                row[cell.Start.Column - 1] = cell.Text;
        //            else
        //                try
        //                {
        //                    row[cell.Start.Column - 1] = cell.Value ?? DBNull.Value;
        //                }
        //                catch
        //                {
        //                    row[cell.Start.Column - 1] = DBNull.Value;
        //                }
        //        }
        //    }
        //}

        //private Type GetColumnType(IEnumerable<ExcelRangeBase> excelRange)
        //{
        //    Type d = typeof(String);
        //    int diffs = 0;
        //    int max = 100;
        //    double dou;
        //    DateTime time = DateTime.MinValue;
        //    foreach (var row in excelRange)
        //    {
        //        //if (time != DateTime.MinValue && !DateTime.TryParse(row.Text, out time))
        //        //    return typeof(String);
              
        //        if ( max-- == 0)
        //            break;
        //        var dd = row.Value?.GetType();//ParseString(row.Text);
        //        if (dd != null && dd != d)
        //        {
        //            if(dd == typeof(double)&&!double.TryParse(row.Text, out dou))
        //                return typeof(String);
        //            d = dd;
        //            diffs++;
        //            if (diffs > 1)
        //                return typeof(String);
        //            //if (d == typeof (Double))
        //            //    DateTime.TryParse(row.Text, out time);
        //        }
                


        //    }

        //    return d; //time!= DateTime.MinValue?typeof(TimeSpan): d;
        //}

        public void ApplyOle(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            ExcelSettings settings = (ExcelSettings)pluginSettings;
            string filename = settings.FileName;
            if (settings.FileName.StartsWith("http") || settings.FileName.StartsWith("ftp"))
            {
                status.InitStatus("Downloading file",0);
                filename = DownloadFile(filename, log);

            }
            status.InitStatus("Loading file", 0);
            string connection = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filename + ";" + "Extended Properties=\"Excel 12.0 Xml;";
            if (filename.EndsWith(".xls"))
                connection = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filename + ";" + "Extended Properties=\"Excel 8.0;";


            // HDR=Yes: the first row, given that it is of datatype string, will be used for naming the columns
            // IMEX=1: a column with mixed data, say double and string, will be of datatype string
            if (settings.Hdr) connection += "HDR=Yes;";
            connection += "IMEX=1;\"";

            OleDbConnection oleDb = new OleDbConnection(connection);
            List<string> sheetNames = new List<string>();
            try
            {
                
                oleDb.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                log.Add(LogType.Error,"Error opening file");
                return;
            }
            if (settings.SheetOption == ExcelSettings.Sheet.All)
            {
                DataTable dt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //sheetNames = new string[dt.Rows.Count];

                foreach (DataRow row in dt.Rows)
                {
                    string name = row["TABLE_NAME"].ToString();
                    if (name[0] == '\'')
                    {
                        // Remove single quotes around numeric sheetnames
                        if (Regex.IsMatch(name, @"^'\d\w+\$'$"))
                            name = name.Substring(1, name.Length - 2);
                    }

                    if (name[name.Length - 1] == '_')
                    {
                        if (!sheetNames.Contains(name.Substring(0, name.Length - 1)))
                        {
                            sheetNames.Add(name);
                        }
                    }
                    else
                    {
                        if (!name.Contains("_FilterDatabase"))
                        {
                            sheetNames.Add(name);
                        }
                    }
                }
            }
            else
            {
                sheetNames.Add(settings.SheetName + "$");
            }

            foreach (string sheet in sheetNames)
            {
                OleDbCommand cmdSelect = new OleDbCommand("SELECT * FROM [" + sheet + "]", oleDb);
                
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmdSelect);
                DataTable table = model.CreateTable();
                table.TableName = sheet.TrimEnd('$');
                
                adapter.Fill(table);
            }
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            oleDb.Close();
            oleDb = null;
            if(Directory.Exists(appData + @"\GDMTool\temp\"))
                Directory.Delete(appData + @"\GDMTool\temp\", true);
            log.Add(LogType.Success, sheetNames.Count + " sheet(s) imported");
        }

        private string DownloadFile(string filename, ILog log)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string newfile = appData + @"\GDMTool\temp\" + Guid.NewGuid() + ".xlsx";
            Directory.CreateDirectory(appData + @"\GDMTool\temp\");
            // Create a new WebClient instance.
            using (WebClient myWebClient = new WebClient())
            {
                try
                {
                    
                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filename, newfile);

                }
                catch (Exception)
                {
                    log.Add(LogType.Error, "Error opening file");
                    return null;
                }
            }
            return newfile;
        }
    

        [Obsolete]
        public void Applyold(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            ExcelSettings settings = (ExcelSettings)pluginSettings;
            string connection = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + settings.FileName + ";" + "Extended Properties=\"Excel 8.0; ";

            // HDR=Yes: the first row, given that it is of datatype string, will be used for naming the columns
            // IMEX=1: a column with mixed data, say double and string, will be of datatype string
            if (settings.Hdr) connection += "HDR=Yes; ";
            connection += "IMEX=1;\"";

            OleDbConnection oleDb = new OleDbConnection(connection);
            List<string> sheetNames = new List<string>();
            oleDb.Open();

            if (settings.SheetOption == ExcelSettings.Sheet.All)
            {
                DataTable dt = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //sheetNames = new string[dt.Rows.Count];

                foreach (DataRow row in dt.Rows)
                {
                    string name = row["TABLE_NAME"].ToString();
                    if (name[0] == '\'')
                    {
                        // Remove single quotes around numeric sheetnames
                        if (Regex.IsMatch(name, @"^'\d\w+\$'$"))
                            name = name.Substring(1, name.Length - 2);
                    }

                    if (name[name.Length - 1] == '_')
                    {
                        if (!sheetNames.Contains(name.Substring(0, name.Length - 1)))
                        {
                            sheetNames.Add(name);
                        }
                    }
                    else
                    {
                        if (!name.Contains("_FilterDatabase"))
                        {
                            sheetNames.Add(name);
                        }
                    }
                }
            }
            else
            {
                sheetNames.Add(settings.SheetName + "$");
            }

            foreach (string sheet in sheetNames)
            {
                OleDbCommand cmdSelect = new OleDbCommand("SELECT * FROM [" + sheet + "]", oleDb);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmdSelect;
                DataTable table = model.CreateTable();
                adapter.Fill(table);
            }

            oleDb.Close();
            oleDb = null;
            log.Add(LogType.Success, sheetNames.Count + " sheet(s) imported");
        }

        public string GetJobDescription(PluginSettings s)
        {
            return ((ExcelSettings)s).Description;
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((ExcelSettings)s).GetDynamicSettings();
        }
    }
}
