using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveTableSettings : PluginSettings
    {
        private string _tableName = null;

        [DisplayName("Table name")]
        [Description("The name of the table to remove.")]
        [Editor(typeof(ListBoxEditor), typeof(UITypeEditor))]

        public string TableName
        {
            get
            {
                
                ListBoxEditor.InitTables(Model);
                return _tableName;
            }
            set { _tableName = value;}
        }

        [Browsable(false)]
        [XmlIgnore]
        public IModel Model
        {
            get; set; }
        public override async Task<bool> IsValid()
        {
            if (_tableName == null)
               throw new PluginException("No table name given. ");
            
            return _tableName != null;
        }
    }
}
