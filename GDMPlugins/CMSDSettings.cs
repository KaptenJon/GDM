using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class CmsdSettings : PluginSettings
    {

        private string _filePath;
        private string _jobTable;
        private string _jobNameRow;
        private string _jobMachineNameRow;
        private string _jobCycleDistirbutionRow;
        private string _mtbfTable;
        private string _mtbfResourceKey;
        private string _mtbfDistributionRow;
        private string _mttrTable;
        private string _mttrResourceKey;
        private string _mttrDistributionRow;
        private string _energyConsumptionTable;
        private string _energyConsumptionResourceRow;
        private string _energyConsumptionStateRow;
        private string _energyConsumptionRow;
        private string _subJobTable;
        private string _SubjobNameRow;
        private string _jobNameRelationRow;
        private string _jobDescriptionRow;
        private string _partConsumedJobRow;
        private string _partConsumedInJobTable;
        private string _partTypeConsumed;
        private string _partProducedJobRow;
        private string _partTypeProduced;
        private string _partProducedTable;
        private string _partTypeTable;
        private string _partTypeId;
        private string _partTypeDescription;
        private string _availabilityTable;
        private string _availabilityResourceKey;
        private string _availabilityDistributionRow;
        private string _assosiatedJobRow;
        private string _scheduleAssosiatedJobTable;
        private string _scheduleIdForAssosiatedJobRow;
        private string _scheduleEndTimeRow;
        private string _scheduleDescriptionRow;
        private string _scheduleTable;
        private string _scheduleStartTimeRow;
        private string _scheduleIdRow;


        [DisplayName("File Path")]
        [Description("The path to the file. The file will be overwritten if it already exists.")]
        [Editor(typeof(SaveFileNameEditor), typeof(UITypeEditor))]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        [DisplayName("1. Job Definition Table")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _jobTable;
            }
            set { _jobTable = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model { get; set; }

        [DisplayName("1.1 Job Name Definition Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobNameRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, JobTable);
                return _jobNameRow;
            }
            set { _jobNameRow = value; }
        }

        [DisplayName("1.2 Resource Name Definition Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobMachineNameRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, JobTable);
                return _jobMachineNameRow;
            }
            set { _jobMachineNameRow = value; }
        }

        [DisplayName("1.3 Job cycle time distirbution row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobCycleDistirbutionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, JobTable);
                return _jobCycleDistirbutionRow;
            }
            set { _jobCycleDistirbutionRow = value; }
        }

        [DisplayName("1.4 Job Description row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobDescriptionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, JobTable);
                return _jobDescriptionRow;
            }
            set { _jobDescriptionRow = value; }
        }

        [DisplayName("2. Table for Part Type Descriptions (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartTypesTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _partTypeTable;
            }
            set { _partTypeTable = value; }
        }

        [DisplayName("2.1 Part Type Identifier Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartTypeId
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partTypeTable);
                return _partTypeId;
            }
            set { _partTypeId = value; }
        }

        [DisplayName("2.2 Part Type Description Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartTypeDescription
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partTypeTable);
                return _partTypeDescription;
            }
            set { _partTypeDescription = value; }
        }

        [DisplayName("3. Table for Part Type Consumed in Job (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartConsumedInJobTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _partConsumedInJobTable;
            }
            set { _partConsumedInJobTable = value; }
        }

        [DisplayName("3.2 Part Type Consumed Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartTypeConsumed
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partConsumedInJobTable);
                return _partTypeConsumed;
            }
            set { _partTypeConsumed = value; }
        }

        [DisplayName("3.1 Job Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartConsumedJobRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partConsumedInJobTable);
                return _partConsumedJobRow;
            }
            set { _partConsumedJobRow = value; }
        }

        [DisplayName("4. Table for Part Type Produced in Job (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartProducedTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _partProducedTable;
            }
            set { _partProducedTable = value; }
        }

        [DisplayName("4.2 Part Type Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartTypeProduced
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partProducedTable);
                return _partTypeProduced;
            }
            set { _partTypeProduced = value; }
        }

        [DisplayName("4.1 Job Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string PartProducedJobRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _partProducedTable);
                return _partProducedJobRow;
            }
            set { _partProducedJobRow = value; }
        }

        [DisplayName("5. Job and Sub Job Definition Table (Optional)")]
        [Description("If there are Jobs thasubjobs to others this table should declare the relation")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string SubJobTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _subJobTable;
            }
            set { _subJobTable = value; }
        }

        [DisplayName("5.1 Job Name Row, (new jobb name)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string JobNameRelationRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, SubJobTable);
                return _jobNameRelationRow;
            }
            set { _jobNameRelationRow = value; }
        }

        [DisplayName("5.2 Sub Job Name Row Related to the Job in 1")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string SubJobNameRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, SubJobTable);
                return _SubjobNameRow;
            }
            set { _SubjobNameRow = value; }
        }




        [DisplayName("6 MTBF Table (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MtbfTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _mtbfTable;
            }
            set { _mtbfTable = value; }
        }

        [DisplayName("6.1 MTBF Resource Row")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        [Description("")]
        [ReadOnly(false)]
        public string MtbfResourceKey
        {
            get
            {
                ListBoxEditor.InitColumns(Model, MtbfTable);
                return _mtbfResourceKey;
            }
            set { _mtbfResourceKey = value; }
        }

        [DisplayName("6.2 MTBF Distribution Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MtbfDistributionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, MtbfTable);
                return _mtbfDistributionRow;
            }
            set { _mtbfDistributionRow = value; }
        }


        [DisplayName("7 MTTR Table (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MttrTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _mttrTable;
            }
            set { _mttrTable = value; }
        }

        [DisplayName("7.1 MTTR Resource Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MttrResourceKey
        {
            get
            {
                ListBoxEditor.InitColumns(Model, MttrTable);
                return _mttrResourceKey;
            }
            set { _mttrResourceKey = value; }
        }

        [DisplayName("7.2 MTTR Distribution Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string MttrDistributionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, MttrTable);
                return _mttrDistributionRow;
            }
            set { _mttrDistributionRow = value; }
        }

        [DisplayName("8 Availability Table (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string AvailabilityTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _availabilityTable;
            }
            set { _availabilityTable = value; }
        }

        [DisplayName("8.1 Availability Resource Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string AvailabilityResourceKey
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _availabilityTable);
                return _availabilityResourceKey;
            }
            set { _availabilityResourceKey = value; }
        }

        [DisplayName("8.2 Availability Value Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string AvailabilityValueRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _availabilityTable);
                return _availabilityDistributionRow;
            }
            set { _availabilityDistributionRow = value; }
        }


        [DisplayName("9 Energy Consumption Table (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string EnergyConsumptionTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _energyConsumptionTable;
            }
            set { _energyConsumptionTable = value; }
        }

        [DisplayName("9.1 Energy Consumption Resource Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string EnergyConsumptionResourceRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, EnergyConsumptionTable);
                return _energyConsumptionResourceRow;
            }
            set { _energyConsumptionResourceRow = value; }
        }

        [DisplayName("9.2 Energy Consumption State Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string EnergyConsumptionStateRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, EnergyConsumptionTable);
                return _energyConsumptionStateRow;
            }
            set { _energyConsumptionStateRow = value; }
        }

        [DisplayName("9.3 Energy Consumption Row")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string EnergyConsumptionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, EnergyConsumptionTable);
                return _energyConsumptionRow;
            }
            set { _energyConsumptionRow = value; }
        }

        [DisplayName("10. Scheduleing Table (Optional)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _scheduleTable;
            }
            set { _scheduleTable = value; }
        }

        [DisplayName("10.1 Scheduling Id")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleIdRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleTable);
                return _scheduleIdRow;
            }
            set { _scheduleIdRow = value; }
        }

        [DisplayName("10.2 Schedule Description")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleDescriptionRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleTable);
                return _scheduleDescriptionRow;
            }
            set { _scheduleDescriptionRow = value; }
        }

        [DisplayName("10.2 Schedule Start Time")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleStartTimeRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleTable);
                return _scheduleStartTimeRow;
            }
            set { _scheduleStartTimeRow = value; }
        }

        [DisplayName("10.2 Schedule Start Time")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleEndTimeRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleTable);
                return _scheduleEndTimeRow;
            }
            set { _scheduleEndTimeRow = value; }
        }

        [DisplayName("11. Scheduleing Table (Optional if no corresponding scheduling exists empty one will be created.)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleAssosiatedJobTable
        {
            get
            {
                ListBoxEditor.InitTables(Model);
                return _scheduleAssosiatedJobTable;
            }
            set { _scheduleAssosiatedJobTable = value; }
        }

        [DisplayName("11.1 Scheduling Id (Optional, generated if not defined)")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string ScheduleIdForAssosiatedJobRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleAssosiatedJobTable);
                return _scheduleIdForAssosiatedJobRow;
            }
            set { _scheduleIdForAssosiatedJobRow = value; }
        }

        [DisplayName("11.2 Assosiated Job")]
        [Description("")]
        [ReadOnly(false)]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public string AssosiatedJobRow
        {
            get
            {
                ListBoxEditor.InitColumns(Model, _scheduleAssosiatedJobTable);
                return _assosiatedJobRow;
            }
            set { _assosiatedJobRow = value; }
        }

        public override async Task<bool> IsValid()
        {
            

            if (JobTable == null)
                throw new PluginException("Table name not set. ");
            if (_filePath == null)
                throw new PluginException( "File path given set. ");
            return true;
        }

        public object GetDynamicSettings()
        {
            return new DynamicSettings(this);
        }

        private class DynamicSettings
        {
            private CmsdSettings _settings;

            public DynamicSettings(CmsdSettings settings)
            {
                _settings = settings;
            }

            [DisplayName("File Path")]
            [Description("The path to the file. The file will be overwritten if it already exists.")]
            [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
            public string FilePath
            {
                get { return _settings.FilePath; }
                set { _settings.FilePath = value; }
            }
        }

        

        public int GetTableType(string tableName)
        {
             if(EnergyConsumptionTable == tableName)
                return 4;
             if(MtbfTable == tableName)
                return 2;
            
             if(MttrTable== tableName)
                return 3;
           
             if (JobTable == tableName)
                return 1;
            return -1;

        }
    }
}
