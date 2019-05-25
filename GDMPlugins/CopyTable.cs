using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CopyTable : ITool
    {
        public string Description => "Copies tables";
        public string Version => "1.0";
        public string Name => "Copy Table";
        public Image Icon => Icons.copy;

        public PluginSettings GetSettings(IModel model)
        {
            return new CopyTableSettings(model);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {

            var s = pluginSettings as CopyTableSettings;
            if (s == null)
                return;
            if (s == null) s = new CopyTableSettings(model);
            s.Model = model;
            s.FromTable = s.FromTable;
            s.ToTable = s.ToTable;
        }

        public Type GetSettingsType()
        {
            return typeof(CopyTableSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var s = pluginSettings as CopyTableSettings;
            if (s == null)
                return;

            DataTable tabel, fromtable = model.GetTable(s.FromTable);
            if (fromtable == null)
                return;
            if (model.GetTable(s.ToTable) != null)
            {
                model.DropTable(s.ToTable);
            }

            tabel = fromtable.Copy();
            tabel.TableName = s.ToTable;
            model.AddTable(tabel);



        }

        public bool NeedColumnSelected => false;
        public bool NeedTableSelected => true;
        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Copy";
    }

    public class CopyTableSettings : PluginSettings
    {
        private IModel _model;
        private string _fromTable;

        public CopyTableSettings()
        {

        }

        public CopyTableSettings(IModel model)
        {
            _model = model;

        }

        [DisplayName("Copy From")]
        [Description("Copy a table to a new one")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]

        public String FromTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _fromTable;
            }
            set { _fromTable = value; }
        }

        [DisplayName("New Table Name")]
        [Description("Copy a table to a new one")]

        public String ToTable { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set { _model = value; }
        }


        public async override Task<bool> IsValid()
        {
            return await Factory.StartNew(() => _model?.GetTables().Count > 0);
        }
    }


    public class CopyColumn : ITool
    {
        public string Description => "Copy a column";
        public string Version => "1.0";
        public string Name => "Copy Column";
        public Image Icon => Icons.copy;

        public PluginSettings GetSettings(IModel model)
        {
            return new CopyColumnSettings(model);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {

            var s = pluginSettings as CopyColumnSettings;
            if (s == null)
                return;
            s.Model = model;
        }

        public Type GetSettingsType()
        {
            return typeof(CopyColumnSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var s = pluginSettings as CopyColumnSettings;
            if (s == null)
                return;

            DataTable fromtable = model.GetTable(s.FromTable), totable = model.GetTable(s.ToTable);
            if (fromtable == null || !fromtable.Columns.Contains(s.FromColumn) || (model.GetTable(s.FromTable).Columns.Contains(s.FromColumn) &&  model?.GetTable(s.FromColumn)?.Columns?[s.FromColumn]?.DataType != model?.GetTable(s.ToTable)?.Columns?[s.ToColumn]?.DataType))
                return;
            if (totable == null)
            {
                totable = model.CreateTable();
                totable.TableName = s.ToTable;
            }
            if (!totable.Columns.Contains(s.ToColumn))
                totable.Columns.Add(s.ToColumn, model?.GetTable(s.FromColumn)?.Columns?[s.FromColumn]?.DataType??typeof(String));

            
            for (int i = 0; fromtable.Rows.Count > i; i ++ )
            {
                DataRow fromrow = fromtable.Rows[i];
                DataRow torow = null;
                if (totable.Rows.Count <= i)
                    torow = totable.NewRow();
                else
                {
                    torow = totable.Rows[i];
                }
          
                torow[s.ToColumn] = fromrow[s.FromColumn];
                if (totable.Rows.Count <= i)
                    totable.Rows.Add(torow);
                
                //torow.AcceptChanges();
            }

        }

        public bool NeedColumnSelected => false;
        public bool NeedTableSelected => true;
        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Copy";
    }

    public class CopyColumnSettings : PluginSettings
    {
        private IModel _model;
        private string _toTable;
        private string _toColumn;
        private string _fromColumn;
        private string _fromTable;


        public CopyColumnSettings()
        {

        }

        public CopyColumnSettings(IModel model)
        {
            _model = model;

        }

        [DisplayName("1.2 Copy From Column")]
        [Description("Copy a table to a new one")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]

        public String FromColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, FromTable);
                return _fromColumn;
            }
            set { _fromColumn = value; }
        }

        [DisplayName("1.1 From From Table")]
        [Description("Copy a column in this table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string FromTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _fromTable;
            }
            set { _fromTable = value; }
        }

        [DisplayName("2.1 To Table")]
        [Description("Copy a column in this table")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ToTable
        {
            get
            {
                ListBoxEditor.InitTables(_model);
                return _toTable;
            }
            set { _toTable = value; }
        }

        [DisplayName("2.2 To Column")]
        [Description("Copy a column to this column, if it does not exsist create a new column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public String ToColumn
        {
            get
            {
                ListBoxEditor.InitColumns(_model, ToTable);
                return _toColumn;
            }
            set { _toColumn = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
            }
        }


        public async override Task<bool> IsValid()
        {
            return await Factory.StartNew(() => FromTable != null&& ToTable != null && ToColumn != null && FromColumn != null);
        }
    }
}
