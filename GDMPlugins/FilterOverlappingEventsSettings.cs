using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace Statistics
{
    public class FilterOverlappingEventsSettings:PluginSettings
    {
        private string _startDateColumn;
        private string _endDateColumn;

        public FilterOverlappingEventsSettings()
        {
            
        }

        [Browsable(true)]
        [DisplayName("2.1 Event Start")]
        [Editor(typeof (ListBoxEditor), typeof (UITypeEditor))]
        public String StartDateColumn
        {
            get
            {
                ListBoxEditor.InitColumns(Model, TargetTable, typeof(DateTime));
                return _startDateColumn;
            }
            set { _startDateColumn = value; }
        }

        [Browsable(true)]
        [DisplayName("3 Group filter on column")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]
        public String GroupColumn
        {
            get
            {
                ListBoxEditor.InitColumns(Model, TargetTable, typeof(string));
                return _groupColumn;
            }
            set { _groupColumn = value; }
        }


        [Browsable(true)]
        [DisplayName("2.2 Event End")]
        [Editor(typeof (ListBoxEditor), typeof (UITypeEditor))]
        public String EndDateColumn
        {
            get
            {
                ListBoxEditor.InitColumns(Model, TargetTable, typeof(DateTime));
                return _endDateColumn;
            }
            set { _endDateColumn = value; }
        }
        [XmlIgnore]
        public IModel Model;
        private string _groupColumn;

        [Browsable(true)]
        [ReadOnly(true)]
        [DisplayName("1 Target table")]
        public String TargetTable
        {
            get; set; 
        }


        public async override Task<bool> IsValid()
        {
            return await Factory.StartNew(() => { return true; });
        }
    }
}
