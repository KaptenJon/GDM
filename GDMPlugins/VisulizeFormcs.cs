using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace GDMPlugins
{
    public partial class VisulizeFormcs : Form
    {
        
        private Datareader _dbr;
        public DataTable _model;
        public static string _columnresource;
        public static string _columnmean;
        public static string _columnstd;
        
        public VisulizeFormcs()
        {
            InitializeComponent();
        }

        public VisulizeFormcs(DataTable db):this()
        {
            //var dr = db.CreateDataReader();
            _model = db;

        }
        private void VisulizeFormcs_Load(object sender, EventArgs e)
        {
            Init();
            
        }

        public void Export(string file)
        {
            Init();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes = this.report.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            try
            {
                using (var f = File.OpenWrite(file))
                {
                    f.Write(bytes, 0, bytes.Length);
                    f.Close();
                }
            }
            catch
            {
                
            }
        }

        private void Init()
        {
            if (_model == null)
                return;
            _dbr = new Datareader(_model);
            _dbr._columnresourcenr = _model.Columns.IndexOf(_columnresource);
            _dbr._columnmeannr = _model.Columns.IndexOf(_columnmean);
            _dbr._columnstdnr = _model.Columns.IndexOf(_columnstd);
            this.visulizationDataset.Load(_dbr, LoadOption.OverwriteChanges, "ResourceActiveTime");
            this.report.Refresh();
            this.report.RefreshReport();
        }

        public class Datareader : IDataReader
        {
            public  int _columnresourcenr = 0;
            public  int _columnmeannr = 1;
            public  int _columnstdnr = 2;

            public DataTable dt;
            private int _row = -1;
            private int _column = 0;
            private VisulizationDataset.ResourceActiveTimeDataTable _schema;

            public Datareader(DataTable dt)
            {
                this.dt = dt.Copy();

            }

            public void Dispose()
            {
                dt.Dispose();
            }

            public string GetName(int i)
            {
                return _schema.Columns[i].ColumnName;
               
            }

            public string GetDataTypeName(int i)
            {
                return _schema.Columns[i].DataType.ToString();
              
            }

            public Type GetFieldType(int i)
            {
                return _schema.Columns[i].DataType;
        }

            public object GetValue(int i)
            {
                ;

                return null;
            }

            public int GetValues(object[] values)

            {
                int i = 0;
                try
                {



                    for (; i < FieldCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                values[i] = dt.Rows[_row][_columnresourcenr] as String;
                                break;
                            case 1:
                                values[i] = dt.Rows[_row][_columnmeannr] as double?;
                                break;
                            case 2:
                                values[i] =
                                    ((double) dt.Rows[_row][_columnmeannr] + (double) dt.Rows[_row][_columnstdnr]) as
                                        double?;
                                break;
                            case 3:
                                values[i] =
                                    ((double) dt.Rows[_row][_columnmeannr] - (double) dt.Rows[_row][_columnstdnr]) as
                                        double?;
                                break;
                            case 4:
                                values[i] = 0d as double?;
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    return 0;
                }

                return i;
            }

            public int GetOrdinal(string name)
            {
                throw new NotImplementedException();
            }

            public bool GetBoolean(int i)
            {
                throw new NotImplementedException();
            }

            public byte GetByte(int i)
            {
                throw new NotImplementedException();
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public char GetChar(int i)
            {
                throw new NotImplementedException();
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public Guid GetGuid(int i)
            {
                throw new NotImplementedException();
            }

            public short GetInt16(int i)
            {
                throw new NotImplementedException();
            }

            public int GetInt32(int i)
            {
                throw new NotImplementedException();
            }

            public long GetInt64(int i)
            {
                throw new NotImplementedException();
            }

            public float GetFloat(int i)
            {
                throw new NotImplementedException();
            }

            public double GetDouble(int i)
            {
                throw new NotImplementedException();
            }

            public string GetString(int i)
            {
                throw new NotImplementedException();
            }

            public decimal GetDecimal(int i)
            {
                throw new NotImplementedException();
            }

            public DateTime GetDateTime(int i)
            {
                throw new NotImplementedException();
            }

            public IDataReader GetData(int i)
            {
                throw new NotImplementedException();
            }

            public bool IsDBNull(int i)
            {
                throw new NotImplementedException();
            }

            public int FieldCount { get; } = 5;

            object IDataRecord.this[int i]
            {
                get { throw new NotImplementedException(); }
            }

            object IDataRecord.this[string name]
            {
                get { throw new NotImplementedException(); }
            }

            public void Close()
            {
                ;
            }

            public DataTable GetSchemaTable()
            {
                _schema = new VisulizationDataset.ResourceActiveTimeDataTable();
                return null;
                
            }

            public bool NextResult()
            {
                if (dt.Rows.Count < _row && dt.Columns.Count < _column)
                    return true;
                return false;
            }

            public bool Read()
            {
                _row++;
                return _row < dt.Rows.Count;
            }

            public int Depth { get; }
            public bool IsClosed { get; } = false;
            public int RecordsAffected { get; }
        }
       
    }
}

