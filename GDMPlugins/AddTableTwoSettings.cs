using System.ComponentModel;
using System.Threading.Tasks;
using GDMInterfaces;

namespace GDMPlugins
{
    
    public class AddTableTwoSettings:PluginSettings
    {
        
        #region Overrides of PluginSettings
        public AddTableTwoSettings()
        {
            
        }

        public AddTableTwoSettings(IModel m)
        {
            Table1Name = m.SelectedTable.TableName;
        }

        [DisplayName("First table name")]
        [Description("The name of the table to apply the plugin onto.")]
        [ReadOnly(false)]
        public string Table1Name { get; set; }

        [DisplayName("Second table name")]
        [Description("The name of the column to apply the plugin onto.")]
        [ReadOnly(false)]
        public string Table2Name { get; set; }

        public override async Task<bool> IsValid()
        {
            
            if (Table1Name != null && Table2Name != null)
                return true;
            return true;
        }
        #endregion
    }
}
