using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class DateDifferenceColumnSettings : PluginSettings
    {
        public enum TimeUnitColumn { Hours, Minutes, Seconds, Milliseconds };
        public enum SortingColumnOption {Increasing, Decreasing}
        private string _table;
        private string _newColumn;
        private TimeUnitColumn _timeFormat;
        private IModel _model;
        private string _secondColumnName;
        private string _firstDateColumn;

        public DateDifferenceColumnSettings(){}

        public DateDifferenceColumnSettings(IModel model)
        {
            _model = model;
        }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [DisplayName("Table Name")]
        [Description("The name of the table to calculate a difference upon.")]
        [ReadOnly(false)]
        public string TableName
        {
            get { return _table; }
            set { _table = value; }
        }



        [DisplayName("Last Date Column")]
        [Description("The Column to calculate the date difference upon.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string SecondDateSecondColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _model.SelectedTable.TableName);
                }
                return _secondColumnName;
            }
            set
            {
                if (_model == null)
                {
                    _secondColumnName = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _model.SelectedTable.TableName, _model))
                    {
                        _secondColumnName = value;
                    }
                }
            }
        }
        [DisplayName("First Date Column")]
        [Description("The Column to calculate the date difference upon.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string FirstDateColumn
        {
            get
            {
                if (_model != null)
                {
                    ListBoxEditor.ColumnsInTable(_model, _model.SelectedTable.TableName);
                }
                return _firstDateColumn;
            }
            set
            {
                if (_model == null)
                {
                    _firstDateColumn = value;
                }
                else
                {
                    if (ListBoxEditor.IsColumn(value, _model.SelectedTable.TableName, _model))
                    {
                        _firstDateColumn = value;
                    }
                }
            }
        }

        [DisplayName("New Column Name")]
        [Description("The name of the new column where the result will be placed.")]
        public string NewColumn
        {
            get { return _newColumn; }
            set { _newColumn = value; }
        }

        [DisplayName("Time Unit")]
        [Description("The unit to be used when calculating the difference.")]
        [ReadOnly(false)]
        public TimeUnitColumn TimeFormat
        {
            get { return _timeFormat; }
            set { _timeFormat = value; }
        }

        public override Task<bool> IsValid()
        {
            
            return Factory.StartNew(() =>
            {
                bool exsists = _model.SelectedTable.Columns.Contains(_newColumn);
                if (FirstDateColumn == null)
                    throw new PluginException("No date column specified. ");
                if (SecondDateSecondColumn == null)
                    throw new PluginException("No valid grouping specified. ");
                if (_newColumn == null)
                    throw new PluginException("No name for new column specified. ");
                if (TableName == null)
                    throw new PluginException("No table specified. ");
                if (_newColumn != null && exsists)
                    throw new PluginException("New column name already exists");
                return true;
            });

        }

        public override string ToString()
        {
            return FirstDateColumn + " " + SecondDateSecondColumn + " " + _newColumn ;
        }

    }

    
}

