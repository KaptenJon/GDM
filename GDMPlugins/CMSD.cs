using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using GDMInterfaces;
using GDMPlugins.Statistics;

namespace GDMPlugins
{
    public class Cmsd : IOutput
    {
        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Output";

        public string Description => "Exports a DataTable to a text file";

        public string Version => "1.0";

        public string Name => "Save to CMSD";

        public Image Icon => Icons.XML;

        public PluginSettings GetSettings(IModel model)
        {
            return new CmsdSettings();
        }

        private string _lastSelectedTable = "";
        private int _lastSelectedTableType = 0;
        private string _lastSelectedColumn = "";
        private int _lastSelectedcolumnType = 0;
        private bool _xmlInit = false;
        private bool _stop = false;
        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            if(!_xmlInit)
            {
                _xmlInit = true;
                return;
            }
            CmsdSettings settings = (CmsdSettings)pluginSettings;
            if (PluginSettings.IsInUIMode)
                settings.Model = model;
            if(model.SelectedTable == null || model.SelectedColumnName == null)
                return;

            if(model.SelectedTable.TableName != _lastSelectedTable && !_stop)
            {
                _lastSelectedTable = model.SelectedTable.TableName;
                _lastSelectedcolumnType = 0;
                _lastSelectedColumn = "";
                switch (_lastSelectedTableType)
                {
                    case 0:
                        settings.JobTable = _lastSelectedTable;
                        _lastSelectedTableType = 1;
                        break;
                    case 4:
                        settings.EnergyConsumptionTable = _lastSelectedTable;
                        _lastSelectedTableType = 5;
                        break;
                    case 2:
                        settings.MtbfTable = _lastSelectedTable;
                        _lastSelectedTableType = 3;
                        break;
                   
                    case 3:
                        settings.MttrTable = _lastSelectedTable;
                        _lastSelectedTableType = 4;
                        break;
                   
                    default:
                        break;
                    

                }
               
            }
            else if(model.SelectedTable.TableName != _lastSelectedTable && _stop)
            {
                _lastSelectedTable = model.SelectedTable.TableName;
                _lastSelectedTableType = settings.GetTableType(model.SelectedTable.TableName);
            }
           
        }

        public Type GetSettingsType()
        {
            return typeof(CmsdSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CmsdSettings settings = (CmsdSettings)pluginSettings;
            
            
            FileStream fs = null;
            
            status.InitStatus("Exporting DataTable...", 10);
            CMSDDocument document = new CMSDDocument();
            CMSDDocumentDataSection dataSection = new CMSDDocumentDataSection();
            document.Items = new object[] {dataSection};
            Dictionary<string, CMSDDocumentDataSectionJob> jobs = new Dictionary<string, CMSDDocumentDataSectionJob>();
            Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
            
            //part types Produced
            var PartProducedInJobs = new Dictionary<string, List<CMSDDocumentDataSectionJobPlannedEffortPartType>>();

            if (!string.IsNullOrWhiteSpace(settings.PartProducedTable) &&
                !string.IsNullOrWhiteSpace(settings.PartProducedJobRow) &&
                !string.IsNullOrWhiteSpace(settings.PartTypeProduced))
            {
                var PartTable = model.GetTable(settings.PartProducedTable);
                foreach (DataRow dataRow in PartTable.Rows)
                {
                    string job = (dataRow[settings.PartProducedJobRow].ToString()).Trim();
                    string part = (dataRow[settings.PartTypeProduced].ToString()).Trim();
                    if(!PartProducedInJobs.ContainsKey(job))
                        PartProducedInJobs.Add(job, new List<CMSDDocumentDataSectionJobPlannedEffortPartType>());
                    PartProducedInJobs[job].Add(new CMSDDocumentDataSectionJobPlannedEffortPartType() { PartTypeIdentifier = part });
                }
            }

            //part types Consumed
            var PartConsumedInJobs = new Dictionary<string, List<CMSDDocumentDataSectionJobPlannedEffortPartType>>();
            

            if (!string.IsNullOrWhiteSpace(settings.PartConsumedInJobTable) &&
                !string.IsNullOrWhiteSpace(settings.PartConsumedJobRow) &&
                !string.IsNullOrWhiteSpace(settings.PartTypeConsumed))
            {
                var PartTable = model.GetTable(settings.PartConsumedInJobTable);
                foreach (DataRow dataRow in PartTable.Rows)
                {
                    string job = (dataRow[settings.PartConsumedJobRow].ToString()).Trim();
                    string part = (dataRow[settings.PartTypeConsumed].ToString()).Trim();
                    if (!PartConsumedInJobs.ContainsKey(job))
                        PartConsumedInJobs.Add(job, new List<CMSDDocumentDataSectionJobPlannedEffortPartType>());
                    PartConsumedInJobs[job].Add(new CMSDDocumentDataSectionJobPlannedEffortPartType() { PartTypeIdentifier = part });
                }
            }

            var PartTypeList =
                PartConsumedInJobs.Values.SelectMany(
                    t => t.Select(p => new PartType() {Identifier = p.PartTypeIdentifier})).ToDictionary(c => c.Identifier, k => k);

            if (!string.IsNullOrWhiteSpace(settings.PartTypesTable) &&
                !string.IsNullOrWhiteSpace(settings.PartTypeDescription) &&
                !string.IsNullOrWhiteSpace(settings.PartTypeId))
            {
                var PartTable = model.GetTable(settings.PartTypesTable);
                foreach (DataRow dataRow in PartTable.Rows)
                {
                    string part = (dataRow[settings.PartTypeId].ToString()).Trim();
                    string desc = (dataRow[settings.PartTypeDescription].ToString()).Trim();
                    if (!PartTypeList.ContainsKey(part))
                    {
                        PartTypeList.Add(part, new PartType() {Identifier = part, Description = desc});
                    }
                    else
                    {
                        PartTypeList[part].Description = desc;
                    }
                    
                         

                }
            }

            dataSection.PartType = PartTypeList.Values.ToArray();

            //Declare Jobs
            DataTable jobtable = model.GetTable(settings.JobTable);
            foreach (DataRow row in jobtable.Rows)
            {
                string id = ((string) row[settings.JobNameRow]).Trim();
                var newjob = new CMSDDocumentDataSectionJob();
                if (jobs.ContainsKey(id))
                {
                    newjob = jobs[id];

                }
                else
                {
                    jobs.Add(id, newjob);
                    newjob.Identifier = id;

                }

                if (newjob.PlannedEffort == null)
                    newjob.PlannedEffort = new CMSDDocumentDataSectionJobPlannedEffort[1];
                newjob.PlannedEffort[0] = new CMSDDocumentDataSectionJobPlannedEffort();
                if (settings.JobCycleDistirbutionRow != null)
                {
                    string unit;
                    newjob.PlannedEffort[0].ProcessingTime = new[]
                    {
                        new CMSDDocumentDataSectionJobPlannedEffortProcessingTime()
                        {
                            Distribution =
                                new[]
                                {GetCMSDDistribution(((string) row[settings.JobCycleDistirbutionRow]).Trim(), out unit)}
                            ,
                            TimeUnit = unit
                        }
                    };

                }
                
                var resource = new Resource() {Identifier = ((string) row[settings.JobMachineNameRow]).Trim()};
                resources.Add(resource.Identifier, resource);
                var resourcepoint = new Resource()
                {
                    ResourceIdentifier = ((string) row[settings.JobMachineNameRow]).Trim()
                };
                newjob.PlannedEffort[0].ResourcesRequired = new Resource[1] {resourcepoint};



            }
            if (settings.SubJobTable != null && settings.SubJobNameRow != null && settings.JobNameRelationRow != null)
            {
                var relation = new Dictionary<string, CMSDDocumentDataSectionJob>();

                DataTable subjobtable = model.GetTable(settings.SubJobTable);
                foreach (DataRow row in subjobtable.Rows)
                {
                    var job = (string) row[settings.JobNameRelationRow].ToString();
                    var subjob = (string) row[settings.SubJobNameRow].ToString();
                    if (!relation.ContainsKey(job)) relation.Add(job,new CMSDDocumentDataSectionJob() {Identifier = job, SubJob = new CMSDDocumentDataSectionJobSubJob[0], PlannedEffort = new CMSDDocumentDataSectionJobPlannedEffort[1] {new CMSDDocumentDataSectionJobPlannedEffort()}});
                    relation[job].SubJob = ArrayMethod<CMSDDocumentDataSectionJobSubJob>.AddToArray(relation[job].SubJob,
                        new CMSDDocumentDataSectionJobSubJob()
                        {
                            JobIdentifier = subjob

                        });
                    

                }
                foreach (var cmsdDocumentDataSectionJob in relation)
                {
                    jobs.Add(cmsdDocumentDataSectionJob.Key, cmsdDocumentDataSectionJob.Value);
                }
               

            }

            foreach (var cmsdDocumentDataSectionJob in jobs)
            {
                if (PartProducedInJobs.ContainsKey(cmsdDocumentDataSectionJob.Key))
                {
                    cmsdDocumentDataSectionJob.Value.PlannedEffort[0].PartTypeProduced = PartProducedInJobs[cmsdDocumentDataSectionJob.Key].ToArray();
                }
                if (PartConsumedInJobs.ContainsKey(cmsdDocumentDataSectionJob.Key))
                {
                    cmsdDocumentDataSectionJob.Value.PlannedEffort[0].PartTypeConsumed = PartConsumedInJobs[cmsdDocumentDataSectionJob.Key].ToArray();
                }
            }


            dataSection.Job = jobs.Values.ToArray();
            if(settings.EnergyConsumptionTable != null)
            {
                foreach (DataRow row in model.GetTable(settings.EnergyConsumptionTable).Rows)
                {
                    if (resources.ContainsKey(((string)row[settings.EnergyConsumptionResourceRow]).Trim()))
                    {
                        var res = resources[((string)row[settings.EnergyConsumptionResourceRow]).Trim()];

                        res.Property = ArrayMethod<ResourceProperty>.AddToArray(res.Property, new ResourceProperty()
                                                                                   {
                                                                                       Name =
                                                                                           "ConsumptionEffect:" +
                                                                                           row[
                                                                                               settings.
                                                                                                   EnergyConsumptionStateRow
                                                                                               ].ToString().Trim(),
                                                                                       Value =
                                                                                           
                                                                                           row[
                                                                                               settings.
                                                                                                   EnergyConsumptionRow].ToString().Trim(),
                                                                                       Unit = "Watt"
                                                                                   });
                    }
                }
            }

            if(settings.MtbfTable!=null)
            {
                foreach (DataRow row in model.GetTable(settings.MtbfTable).Rows)
                {
                    if (resources.ContainsKey((string) row[settings.MtbfResourceKey]))
                    {
                        string unit;
                        Distribution dist = GetCMSDDistribution(((string)row[settings.MtbfDistributionRow]).Trim(),out unit);
                        
                        var res = resources[((string)row[settings.MtbfResourceKey]).Trim()];
                        var prop = new ResourceProperty() { Name = "MTBF", Unit = unit , Distribution = new Distribution[] { dist } };
                        res.Property = ArrayMethod<ResourceProperty>.AddToArray(res.Property, prop);
                    }
                }
            }

            if(settings.MttrTable!=null)
            {
                foreach (DataRow row in model.GetTable(settings.MttrTable).Rows)
                {
                    if (resources.ContainsKey(((string)row[settings.MttrResourceKey]).Trim()))
                    {
                        string unit;
                        Distribution dist = GetCMSDDistribution(((string)row[settings.MttrDistributionRow]).Trim(),out unit);

                        var res = resources[((string) row[settings.MttrResourceKey]).Trim()];
                        var prop = new ResourceProperty() { Name = "MTTR", Unit = unit, Distribution = new Distribution[] { dist } };
                        res.Property = ArrayMethod<ResourceProperty>.AddToArray(res.Property, prop);
                    }
                }
            }

            if (settings.AvailabilityTable != null)
            {
                foreach (DataRow row in model.GetTable(settings.AvailabilityTable).Rows)
                {
                    if (resources.ContainsKey(((string)row[settings.AvailabilityResourceKey]).Trim()))
                    {
                        
                        var res = resources[((string)row[settings.AvailabilityResourceKey]).Trim()];
                        var prop = new ResourceProperty() { Name = "Availability", Value = row[settings.AvailabilityValueRow].ToString().Trim()};
                        res.Property = ArrayMethod<ResourceProperty>.AddToArray(res.Property, prop);
                    }
                }
            }

            dataSection.Resource = resources.Values.ToArray();


            //Schedule
            var Schedules = new Dictionary<string, Schedule>();
            int incId = 0;
            if (!string.IsNullOrWhiteSpace(settings.ScheduleAssosiatedJobTable) &&
                !string.IsNullOrWhiteSpace(settings.AssosiatedJobRow))
            {
                var schedulesItems = model.GetTable(settings.ScheduleAssosiatedJobTable);
                foreach (DataRow dataRow in schedulesItems.Rows)
                {
                    string assosiatedjob = (dataRow[settings.AssosiatedJobRow].ToString()).Trim();
                    string scheduleId = string.IsNullOrWhiteSpace(settings.ScheduleIdForAssosiatedJobRow)? "1": (dataRow[settings.ScheduleIdForAssosiatedJobRow].ToString()).Trim();
                    if (!Schedules.ContainsKey(scheduleId))
                        Schedules.Add(scheduleId, new Schedule() {ScheduleItem = new ScheduleItem[0], Identifier = scheduleId });
                    Schedules[scheduleId].ScheduleItem = ArrayMethod<ScheduleItem>.AddToArray(Schedules[scheduleId].ScheduleItem, new ScheduleItem() {AssosiatedJob = assosiatedjob, Identifier = ""+incId++ });
                }

                if (!string.IsNullOrWhiteSpace(settings.ScheduleTable))
                {

                    var schedules = model.GetTable(settings.ScheduleTable);
                    foreach (DataRow dataRow in schedules.Rows)
                    {
                        string description = string.IsNullOrWhiteSpace(settings.ScheduleDescriptionRow)? null: (dataRow[settings.ScheduleDescriptionRow].ToString()).Trim();
                        string scheduleId = string.IsNullOrWhiteSpace(settings.ScheduleIdForAssosiatedJobRow)? Schedules.Keys.FirstOrDefault()??"1": (dataRow[settings.ScheduleIdForAssosiatedJobRow].ToString()).Trim();
                        if (!Schedules.ContainsKey(scheduleId))
                            Schedules.Add(scheduleId, new Schedule() {ScheduleItem = new ScheduleItem[0], Identifier = scheduleId});

                        Schedules[scheduleId].Description = description;

                        if (!string.IsNullOrWhiteSpace(settings.ScheduleStartTimeRow) &&
                            !string.IsNullOrWhiteSpace(settings.ScheduleEndTimeRow ))
                        {

                            DateTime start, end;
                            if(DateTime.TryParse(dataRow[settings.ScheduleStartTimeRow].ToString().Trim(),out start))
                                Schedules[scheduleId].StartTime = start.ToString(CultureInfo.InvariantCulture);
                            else
                            {
                                Schedules[scheduleId].StartTime =
                                    dataRow[settings.ScheduleStartTimeRow].ToString().Trim();
                            }
                            if (DateTime.TryParse(dataRow[settings.ScheduleEndTimeRow].ToString().Trim(), out end))
                                Schedules[scheduleId].EndTime = end.ToString(CultureInfo.InvariantCulture);
                            else
                            {
                                Schedules[scheduleId].EndTime =
                                    dataRow[settings.ScheduleEndTimeRow].ToString().Trim();
                            }
                        }
                        if(string.IsNullOrWhiteSpace(settings.ScheduleIdForAssosiatedJobRow))
                            break;

                    }
                }
            }

            dataSection.Schedule = Schedules.Values.ToArray();


            fs = new FileStream(settings.FilePath, FileMode.Create);
            Utf8StringWriter writer = new Utf8StringWriter(fs);
            

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); 
 
            ns.Add("", ""); 

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CMSDDocument));
            xmlSerializer.Serialize(writer,document,ns);

            writer.Close();
            fs.Close();
            Thread.CurrentThread.CurrentCulture = info;
            log.Add(LogType.Success, " rows written to file");
        }

        private static Distribution GetCMSDDistribution(String distributionString, out string unit)
        {

       
            var distribution = new Distribution();
            var from = distributionString.IndexOf('(');
            var to = distributionString.IndexOf(')');
            var para = distributionString.Substring(from,to-from).Replace("(", "").Replace(")", "").Split(',');
            
            distribution.Name = distributionString.Substring(0, distributionString.IndexOf('(')).Trim();

            List<DistributionDistributionParameter> parameters = new List<DistributionDistributionParameter>();
            
            foreach (var s in para)
            {
                var split = s.Split('=');
                if(split.Length<2)
                    break;
                parameters.Add(new DistributionDistributionParameter() {Name = split[0].Trim(), Value = split[1].Trim()});
            }

            distribution.DistributionParameter = parameters.ToArray();
            var possunit = distributionString.Substring(to);
            if (possunit.Contains("s"))
                unit = "second";
            else if (possunit.Contains("h"))
                unit = "hour";
            else if (possunit.Contains("m"))
                unit = "min";
            else if (possunit.Contains("d"))
                unit = "day";
            else if (possunit.Contains("ms"))
                unit = "millisecond";
            else
                unit = "";
            return distribution;
        }

        public Tag Tags => null;

        public string GetJobDescription(PluginSettings s)
        {
            return "test";
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return ((CmsdSettings)s).GetDynamicSettings();
        }

        public class Utf8StringWriter : StreamWriter
        {
            public Utf8StringWriter(Stream s) : base(s)
            {
            }

            public override Encoding Encoding => Encoding.UTF8;
        } 


        public class ArrayMethod<T>
        {
            public static T[] AddToArray(T[] array , T newT)
            {
                if(array==null)
                    array = new T[1];
                else
                    Array.Resize(ref array, array.Count()+1);
                array[array.Count() - 1] = newT;
                return array;
            }
        }


    }

}
