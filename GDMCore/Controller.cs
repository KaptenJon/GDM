using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// The link between the user interface, the plug-in
    /// and the model (data). User interface components registers for events that is
    /// raised by the controller when the state and data is changed.
    /// </summary>
    public class Controller
    {
        public delegate void ExceptionRaisedDelegate(ILog log);
        public event ExceptionRaisedDelegate ExceptionRaised;

        public delegate void StatusUpdateDelegate(string label, int percent);
        public event StatusUpdateDelegate StatusUpdate;

        public delegate void BatchStatusUpdateDelegate(int number, int percent);
        public event BatchStatusUpdateDelegate BatchStatusUpdate;

        public delegate void ConfigUpdated();
        public event ConfigUpdated CurrentConfigUpdated;

        public delegate void ColumnSelectionChange(string column, Type type);
        public event ColumnSelectionChange ColumnSelection;

        public delegate void TableSelectionChange(string table);
        public event TableSelectionChange TableSelection;

        public delegate void ActivePluginChange(IPlugin plugin);
        public event ActivePluginChange PluginActivated;

        private delegate void PluginOperation(IPlugin plugin, PluginSettings settings);

        public delegate void ModelChangedEvent();
        public event ModelChangedEvent ModelChanged;

        public delegate void ModelChangingEvent();
        public event ModelChangingEvent ModelChanging;

        private static string PluginPath = @"\" + "Plugin";
        public static string ConfigPath = @"\" + "Configurations";

        private IPlugin _activePlugin;
        private PluginSettings _activeSettings;
        private Model _model;
        private ConfigManager _configManager;
        private static PluginManager _pluginManager;
        private Log _log;

        public Log Log
        {
            get { return _log; }
            set { _log = value; }
        }
        private Status _status;

        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }
        private BackgroundWorker _backgroundWorker;
        private List<Tag> _tags;
        private const bool DoCatchExceptions = false;    // false makes it easier to debug
        private bool _exceptionCatched = false;


        public Controller(string path)
            : this(path, path + ConfigPath)
        {

        }
        public static AppConfigXMLStore _GdmConfig;

        public static string GdmServicePath
        {
            get
            {
                if (_GdmConfig == null)
                    if (File.Exists(ConfigPath + @"\appconfig\gdmservice.xml"))
                    {
                        try
                        {
                            using (FileStream stream = File.OpenRead(ConfigPath + @"\appconfig\gdmservice.xml"))
                            {
                                XmlSerializer ser = new XmlSerializer(typeof (AppConfigXMLStore));
                                var obj = (AppConfigXMLStore) ser.Deserialize(stream);
                                _GdmConfig = obj;
                               
                            }
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                return _GdmConfig?.GDMServicePath;
            }
            set
            {
                if(value != null)
               Directory.CreateDirectory(ConfigPath + @"\appconfig");
                using (XmlTextWriter writer = new XmlTextWriter(ConfigPath + @"\appconfig\gdmservice.xml", Encoding.UTF8))
                {
                    if(_GdmConfig == null)
                            _GdmConfig = new AppConfigXMLStore();
                    _GdmConfig.GDMServicePath = value;
                    XmlSerializer ser = new XmlSerializer(typeof(AppConfigXMLStore));
                    writer.Formatting = Formatting.Indented;
                    ser.Serialize(writer, new AppConfigXMLStore() { GDMServicePath = value });
                }
            }
        }
        public Controller(string path, string configPath)
        {
            Path = path;
            if (configPath == null)
                configPath = path + ConfigPath;
            ConfigPath = configPath;


            _model = new Model();
            _pluginManager = new PluginManager(path + PluginPath);
            _log = new Log(this);
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;

            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _status = new Status(_backgroundWorker);
            
            _tags = new List<Tag>();

            foreach (IOutput plugin in _pluginManager.OutputPlugins)
            {
                if (plugin.Tags == null) continue;
                _tags.Add(plugin.Tags);
            }
            _configManager = new ConfigManager(configPath, PluginManager.PluginSettingsType);
        }

        private string Path { get; }

        void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                if (BatchStatusUpdate!=null)
                    BatchStatusUpdate((int)e.UserState, e.ProgressPercentage);
            }
            else
            {
                if (StatusUpdate != null)
                    StatusUpdate(_status.Label, e.ProgressPercentage);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (DoCatchExceptions)
#pragma warning disable 162
            {
                try
                {
                    DoWork(sender, e);
                }
                catch (Exception exception)
                {
                    _log.Add(LogType.Error, exception);
                    _exceptionCatched = true;
                }
            }
#pragma warning restore 162
            else
            {
                DoWork(sender, e);

            }
          
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                Config config = e.Argument as Config;

                if (config != null)
                {
                    int total = config.Operations.Count;
                    int number = 0;

                    foreach (Operation op in config.Operations)
                    {
                        number++;
                        IPlugin plugin = PluginManager.GetPlugin(op.Plugin);
                        _log.CreateChapter(plugin.Name);
                        var start = DateTime.Now;
                        
                        ApplyPlugin(plugin, op.Settings);
                        BatchPlugin.CurrentBatchPlugin = null;
                        op.LastTime = DateTime.Now - start;
                        _backgroundWorker.ReportProgress(number * 100 / total, number);
                    }
                }
                else
                {
                    _log.CreateChapter(_activePlugin.Name);
                    PluginOperation operation = e.Argument as PluginOperation;
                    PluginSettings settings = _activeSettings;
                    _activeSettings = null;//_activePlugin.GetSettings(_model);
                    operation?.Invoke(_activePlugin, settings);
                }
            }
            
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ModelChanged != null) ModelChanged();
            if (CurrentConfigUpdated != null) CurrentConfigUpdated();
            if (_exceptionCatched)
            {
                ExceptionRaised(_log);
                _exceptionCatched = false;
            }
            ResetEvent.Set();
        }

        public bool IsBusy => _backgroundWorker.IsBusy;

        public Model Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public DataTable SelectedTable
        {
            get { return _model.SelectedTable; }
            set
            {
                _model.SelectedTable = value;
                TableSelection(value.TableName);
            }
        }

        public ConfigManager ConfigManager
        {
            get { return _configManager; }
            set { _configManager = value; }
        }

        public static PluginManager PluginManager => _pluginManager;

        public IPlugin ActivePlugin
        {
            get { return _activePlugin; }
            set
            {
                _activePlugin = value;
                if (_activePlugin != null)
                {
                    if (BatchPlugin.CurrentBatchPlugin == null && BatchWorkUpdated != null)
                        BatchWorkUpdated(BatchPlugin.CurrentBatchPlugin, new BatchWorkUpdatedArgs(false, ""));
                    _activeSettings = _activePlugin.GetSettings(_model);
                }
                else
                {
                    _activeSettings = null;
                }
                PluginActivated(value);
            }
        }

        public PluginSettings ActiveSettings
        {
            get
            {
                if (_activeSettings != null)
                {
                    if (!IsBusy)
                    {
                        _activePlugin.UpdateSettings(_activeSettings, _model);
                    }
                    return _activeSettings;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsFirstRun
        {
            get { return !new FileInfo(Path + "\\" + Assembly.GetExecutingAssembly().ImageRuntimeVersion).Exists; }
            set
            {
                if (!value)
                    new FileInfo(Path + "\\" + Assembly.GetExecutingAssembly().ImageRuntimeVersion).Create();
                
            }
        }

        public void SetSelectedColumn(string column, Type type)
        {
            _model.SelectedColumnName = column;
            _model.SelectedColumnType = type;
            if (!IsBusy) ColumnSelection(column, type);
        }

        public void WriteLog(string filename)
        {
            _log.Flush(filename);
        }

        public void ApplyPlugin(IPlugin plugin, PluginSettings settings)
        {
            _log.Add(LogType.Note, "starts executing " + DateTime.Now.ToString("HH:mm:ss.fff"));
            var start = DateTime.Now;

            if (BatchPlugin.CurrentBatchPlugin != null )
            {
                
                BatchPlugin.ApplyPluginToVizulizeBatch(plugin, settings, _log, _status,_model);
                // _configManager.CurrentConfig.AddOperation(plugin, settings, DateTime.Now - start);
            }
            else
            {
                PluginSettings applySettings;
                using (var ms = new MemoryStream())
                {
                    XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode);
                    XmlSerializer ser = new XmlSerializer(typeof(PluginSettings), PluginManager.PluginSettingsType);
                    ser.Serialize(writer, settings);
                    ms.Position = 0;
                    string s;
                    using (var reader = new StreamReader(ms))
                    {
                        s = reader.ReadToEnd();
                    }
                    s = Controller.StringDateTimeFormat(s);

                   var smlreader = new XmlTextReader(new StringReader(s));
                    applySettings = ser.Deserialize(smlreader) as PluginSettings;
                }
                if(applySettings is BatchPluginSettings)
                    BatchPlugin.CurrentBatchPlugin = (BatchPluginSettings)settings;
                plugin.Apply(_model, applySettings, _log, _status);
                if(BatchPlugin.CurrentBatchPlugin != null && BatchWorkUpdated !=null)
                    BatchWorkUpdated(BatchPlugin.CurrentBatchPlugin, new BatchWorkUpdatedArgs(true, "") );
                    
                _configManager.CurrentConfig.AddOperation(plugin, settings, DateTime.Now - start);
            }
            _log.Add(LogType.Note, "finished executing " + DateTime.Now.ToString("HH:mm:ss.fff"));
        }



        public void ApplyActivePlugin()
        {
            if (ModelChanging != null) ModelChanging();
            PluginOperation p = ApplyPlugin;
            
            if (!IsBusy) _backgroundWorker.RunWorkerAsync(p);
        }

        public void ResetData()
        {
            if (ModelChanging != null) ModelChanging();
            _log.Clear();
            ConfigManager.CurrentConfig = new Config();
            _model.ResetData();
            if (ModelChanged != null) ModelChanged();
        }
        public  AutoResetEvent ResetEvent = new AutoResetEvent(false);
        
        private static object _locker = new object();


        public event BatchWorkStatus BatchWorkUpdated;
            public delegate void BatchWorkStatus(object sender, BatchWorkUpdatedArgs args);

        public class BatchWorkUpdatedArgs
        {
            public BatchWorkUpdatedArgs(bool active, string name)
            {
                this.active = active;
                Name = name;
            }

            public string Name;
            public bool active;
        }

        public void BatchRun(Config config)
        {
            
            
            lock (_locker)
            {
                

                CurrentDateFormat(config);
                config.LoadSavedConfigTags(_tags);
                if (_backgroundWorker.IsBusy) ResetEvent.WaitOne();
                    _backgroundWorker.RunWorkerAsync(config);
            }
        }

        private static Config CurrentDateFormat(Config config)
        {
            using (var ms = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode);
                XmlSerializer ser = new XmlSerializer(typeof(Config),PluginManager.PluginSettingsType);
                ser.Serialize(writer, config);


                ms.Position = 0;
                string s;
                using (var reader = new StreamReader(ms))
                {
                    s = reader.ReadToEnd();
                }
                s = StringDateTimeFormat(s);

                //var smlreader = new XmlTextReader(new StringReader(s));
                config = ser.Deserialize(new StringReader(s)) as  Config;
            }
            return config;
        }

        public static string StringDateTimeFormat(string s)
        {
            s = s.Replace("[CurrentDateTime]", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            try
            {
                var startSubstring = s.IndexOf("[CurrentDateTime-", StringComparison.Ordinal);
                var endSubstring = s.IndexOf("]", startSubstring, StringComparison.Ordinal);
                while (startSubstring > 0 && startSubstring < s.Length && endSubstring > startSubstring &&
                       endSubstring < s.Length)
                {
                    if (endSubstring > startSubstring + 16)
                    {
                        string part = s.Substring(startSubstring, endSubstring - startSubstring - 17);
                        if (part.StartsWith("[CurrentDateTime-"))
                        {
                            TimeSpan t;

                            if (TimeSpan.TryParseExact(part.Substring(17)
                                ,
                                new[] {"ddd hh:mm:ss", "d h:m:s", "hh:mm:ss", "h:m:s" },
                                DateTimeFormatInfo.InvariantInfo, out t))
                            {
                                s = s.Replace(part, (DateTime.Now - t).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                    startSubstring = s.IndexOf("[CurrentDateTime-", StringComparison.Ordinal);
                    endSubstring = s.IndexOf("]", startSubstring, StringComparison.Ordinal);
                }
            }
            catch
            {
            }
            return s;
        }

        public List<DataTable> GetTables()
        {
            return _model.GetTables();
        }

        public void SetTag(Tag t)
        {
            if (t.TagType == TagType.Table)
            {
                t.Entity = _model.SelectedTable.TableName;
            }
            else
            {
                t.Entity = _model.SelectedColumnName;
            }
            ConfigManager.CurrentConfig.RefreshTags(_tags);
        }

        public void UnsetTag(Tag t)
        {
            t.Entity = null;
            if (t.TagType == TagType.Table)
            {
                if (t.Subtags != null) {
                    foreach (Tag subtag in t.Subtags)
                    {
                        UnsetTag(subtag);
                    }
                }
            }
            ConfigManager.CurrentConfig.RefreshTags(_tags);
        }

        public List<Tag> GetAvailabeTags(string table, string column, Type type)
        {
            List<Tag> tabletags = GetTableTags(table);
            List<Tag> tags = new List<Tag>();

            foreach (Tag tabletag in tabletags)
            {
                if (tabletag.Subtags != null)
                {
                    foreach (Tag t in tabletag.Subtags) 
                    {
                        if (t.Entity != null) continue;

                        bool a = (t.TagType == TagType.String);
                        bool b = (type == typeof(int) && t.TagType == TagType.Integer);
                        bool c = (type == typeof(double) && t.TagType == TagType.Double);
                        bool d = (type == typeof(double) || type == typeof(int) && t.TagType == TagType.Numeric);

                        if (a || b || c || d) tags.Add(t);
                    }
                }
            }
            return tags;
        }

        public List<Tag> GetAvailabeTags(string table)
        {
            List<Tag> tags = new List<Tag>();
            
            foreach (Tag roottag in _tags)
            {
                foreach (Tag t in roottag.Subtags)
                {
                    if (t.TagType == TagType.Table && t.Entity == null)
                    {
                        tags.Add(t);
                    }
                }
            }

            DataTable datatable = _model.GetTable(table);

            foreach (DataRelation r in datatable.ParentRelations)
            {
                List<Tag> parentTags = GetTableTags(r.ParentTable.TableName);

                foreach(Tag parent in parentTags) 
                {
                    if (parent.Subtags != null)
                    {
                        foreach (Tag parentSubtag in parent.Subtags)
                        {
                            if (parentSubtag.TagType == TagType.Table && parentSubtag.Entity == null)
                            {
                                tags.Add(parentSubtag);
                            }
                        }
                    }
                }
            }

            return tags;
        }

        public List<Tag> GetColumnTags(string column, string table)
        {
            List<Tag> tags = new List<Tag>();
            foreach (Tag t in _tags) GetColumnTags(column, table, t, tags);
            return tags;
        }

        public List<Tag> GetTableTags(string table)
        {
            List<Tag> tags = new List<Tag>();
            foreach (Tag t in _tags) GetTableTags(table, t, tags);
            return tags;
        }

        private void GetColumnTags(string column, string table, Tag t, List<Tag> tags)
        {
            if (t.Subtags != null)
            {
                foreach (Tag subtag in t.Subtags)
                {
                    if (subtag.Entity == column && t.TagType == TagType.Table && t.Entity == table)
                    {
                        tags.Add(subtag);
                    }
                    else
                    {
                        GetColumnTags(column, table, subtag, tags);
                    }
                }
            }
        }

        private void GetTableTags(string table, Tag t, List<Tag> tags)
        {
            if (t.TagType == TagType.Table && t.Entity == table)
            {
                tags.Add(t);
            }

            if (t.Subtags != null)
            {
                foreach (Tag subtag in t.Subtags)
                {
                    GetTableTags(table, subtag, tags);
                }
            }
        }

        public void EndBatch()
        {
            if (ModelChanging != null) ModelChanging();
            BatchPlugin.ExecuteAllButFirst(Model, Log, Status);
            if (ModelChanged != null) ModelChanged();

            BatchWorkUpdated(BatchPlugin.CurrentBatchPlugin, new BatchWorkUpdatedArgs(false, ""));
            BatchPlugin.CurrentBatchPlugin = null;
            
        }



        public void StartBatch(string[] fileNames)
        {
            var plug = PluginManager.GetPlugin("Batch Plugin");
            ActivePlugin = plug;

            var batchPluginSettings = _activeSettings as BatchPluginSettings;
            batchPluginSettings.InitConstant();
            if (batchPluginSettings != null)
                batchPluginSettings.ConstantValue.AddRange(fileNames);
            
            if (ModelChanged != null) ModelChanged();
            //ActivePlugin.UpdateSettings(BatchPlugin.CurrentBatchPlugin, null);
        }
    }


}