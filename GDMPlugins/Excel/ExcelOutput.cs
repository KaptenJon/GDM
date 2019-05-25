using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using DocumentFormat.OpenXml.Packaging;
using GDMInterfaces;

namespace GDMPlugins.Excel
{
    
    public class ExcelOutput : IOutput
    {
        public ExcelOutput()
        {
            
        }
        public string Description => "Saves an Datatable into an Excel file ";

        public string Version => "1.0";

        public string Name => "Save to Excel";
        
        public Image Icon => Icons.Excel;

        

        public PluginSettings GetSettings(IModel model)
        {
            
            var e = new ExcelOutputSettings();
            
            return e;
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            var setting = pluginSettings as ExcelOutputSettings;
            if(setting == null)
                return;
            
            setting.Table = model.SelectedTable?.TableName;
            //setting.ShowDialog();

        }

        

        public Type GetSettingsType()
        {
            return typeof(ExcelOutputSettings);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pluginSettings"></param>
        /// <param name="log"></param>
        /// <param name="status"></param>
        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {


            var set = pluginSettings as ExcelOutputSettings;
            if (set == null) return;
            var i = model?.GetTable(set.Table);
            if (i == null) return;
            var file = new FileInfo(set.FileName);
            if (file.Exists) file.Delete();
            var sep = Thread.CurrentThread.CurrentCulture;
            var uisep = Thread.CurrentThread.CurrentUICulture;

            if (set.Decimal.Contains(","))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("sv-SE");
            }
            if (set.Decimal.Contains("."))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            }
            try
            {
                var stream = file.Create();
                using (
                    var workbook = SpreadsheetDocument.Create(stream,
                        DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    uint sheetId = 1;

                    var table = i;
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets =
                        workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>()
                                .Select(s => s.SheetId.Value)
                                .Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                    {
                        Id = relationshipId,
                        SheetId = sheetId,
                        Name = table.TableName,
                        
                    };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }
                    
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

                            var pp = dsrow[col];
                            var t = pp.GetType();

                            if (t == typeof (DateTime))
                            {
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                            }
                            else if (t == typeof (double) || t == typeof (int))
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                            else
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()??"");

                            newRow.AppendChild(cell);
                        }
                        sheetData.Append(newRow);

                    }
                    workbook.Close();
                }
                stream.Close();
            }
            catch (Exception e)
            {
                log.Add(LogType.Error, e.Message);
                throw;
            }


            // using (excelpackage pck = new excelpackage(new fileinfo(set.filename)))
            //{
            //    excelworksheet ws = pck.workbook.worksheets.add(set.table);

            //    ws.cells["a1"].loadfromdatatable(i, true);
            //    ws.cells.autofitcolumns();
            //    pck.save();
            //}
            Thread.CurrentThread.CurrentCulture = sep;
            Thread.CurrentThread.CurrentUICulture = uisep;
        }
    



        public string GetJobDescription(PluginSettings s)
        {
            return ((ExcelSettings)s).Description;
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((ExcelSettings)s).GetDynamicSettings();
        }


        public Tag Tags { get; } =null;
    }
}
