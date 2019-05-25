using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    public class PiDataValueCorrectorSettings : PluginSettings
    {
        private string _shiftTable;
        private string _shiftLineColumn;
        private string _shiftDayColumn;
        private string _starttimeColumn;
        private string _stopptimeColumn;
        private string _piDataTable;
        private string _piLineColumn;
        private string _piDateColumn;
        private string _piTTRColumn;

        private IModel _model;

        public PiDataValueCorrectorSettings() { }

        public PiDataValueCorrectorSettings(IModel model)
        {
            _model = model;
        }

        [Description("The table containing the shift times.")]
        [DisplayName("Shift times table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ShiftTable
        {
            get
            {
                if (_model != null) ListBoxEditor.Tables(_model);
                return _shiftTable;
            }
            set
            {
                if (_model == null)
                {
                    _shiftTable = value;
                }
                else
                {
                    if (ListBoxEditor.IsTable(value, _model))
                    {
                        _shiftTable = value;
                    }
                }
            }
        }

        [Description("Line column, the column containing the line name in the shift table.")]
        [DisplayName("Shift, Line Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ShiftLine
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _shiftTable);
                }
                return _shiftLineColumn;
            }
            set
            {
                if (_model == null)
                {
                    _shiftLineColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _shiftTable, _model))
                    {
                        _shiftLineColumn = value;
                    }
                }
            }
        }

        [Description("Day column, the column containing the day the shift started.")]
        [DisplayName("Shift, Day Column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string DayColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _shiftTable);
                }
                return _shiftDayColumn;
            }
            set
            {
                if (_model == null)
                {
                    _shiftDayColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _shiftTable, _model))
                    {
                        _shiftDayColumn = value;
                    }
                }
            }
        }

        [Description("Start time, the column containing the start time of the shift.")]
        [DisplayName("Shift, Start time")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string StartTime
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _shiftTable);
                }
                return _starttimeColumn;
            }
            set
            {
                if (_model == null)
                {
                    _starttimeColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _shiftTable, _model))
                    {
                        _starttimeColumn = value;
                    }
                }
            }
        }

        [Description("Stop time, the column containing the stop time of the shift.")]
        [DisplayName("Shift, Stop time")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string StopTime
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _shiftTable);
                }
                return _stopptimeColumn;
            }
            set
            {
                if (_model == null)
                {
                    _stopptimeColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _shiftTable, _model))
                    {
                        _stopptimeColumn = value;
                    }
                }
            }
        }

        [Description("The table containing the PI-data.")]
        [DisplayName("PI data table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PiTable
        {
            get
            {
                if (_model != null) ListBoxEditor.Tables(_model);
                return _piDataTable;
            }
            set
            {
                if (_model == null)
                {
                    _piDataTable = value;
                }
                else
                {
                    if (ListBoxEditor.IsTable(value, _model))
                    {
                        _piDataTable = value;
                    }
                }
            }
        }

        [Description("Line column, the column containing the line name in the shift table.")]
        [DisplayName("PI, Line column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PiLineColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _piDataTable);
                }
                return _piLineColumn;
            }
            set
            {
                if (_model == null)
                {
                    _piLineColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _piDataTable, _model))
                    {
                        _piLineColumn = value;
                    }
                }
            }
        }

        [Description("Date column, the column containing the date in the PI table.")]
        [DisplayName("PI, Date column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PiDateColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _piDataTable);
                }
                return _piDateColumn;
            }
            set
            {
                if (_model == null)
                {
                    _piDateColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _piDataTable, _model))
                    {
                        _piDateColumn = value;
                    }
                }
            }
        }

        [Description("TTR column, the column containing the TTR value in the PI table.")]
        [DisplayName("PI, TTR column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PittrColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _piDataTable);
                }
                return _piTTRColumn;
            }
            set
            {
                if (_model == null)
                {
                    _piTTRColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _piDataTable, _model))
                    {
                        _piTTRColumn = value;
                    }
                }
            }
        }

        public override async Task<bool> IsValid()
        {
            var errorMsg = "";

            if (_piDataTable == null)
                errorMsg += "The PI table name was not set. ";
            if (_piDateColumn == null)
                errorMsg += "The PI date column name was not set. ";
            if (_piLineColumn == null)
                errorMsg += "The PI line column name was not set. ";
            if (_piTTRColumn == null)
                errorMsg += "The PI TTR column name was not set. ";
            if (_shiftDayColumn == null)
                errorMsg += "The shift day column name was not set. ";
            if (_shiftLineColumn == null)
                errorMsg += "The shift line column name was not set. ";
            if (_shiftTable == null)
                errorMsg += "The shift table was not set. ";
            if (_starttimeColumn == null)
                errorMsg += "The shift start time column was not set. ";
            if (_stopptimeColumn == null)
                errorMsg += "The shift stop time column was not set. ";
            if (errorMsg != "")
                throw new PluginException(errorMsg);
            return !(errorMsg.Length > 0);
        }
    }
}