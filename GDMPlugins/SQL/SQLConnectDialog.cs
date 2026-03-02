using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GDMPlugins.SQL
{
    public class SqlConnectDialog : UITypeEditor
    {
        public static List<string> List;

        public static string ConnectionString { get; private set; }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = provider?.GetService(typeof(IWindowsFormsEditorService))
                                as IWindowsFormsEditorService;
            if (editorService == null)
                return value;

            using (var form = new SqlConnectionForm(value as string ?? ConnectionString))
            {
                if (editorService.ShowDialog(form) == DialogResult.OK)
                {
                    ConnectionString = form.ConnectionString;
                    return ConnectionString;
                }
            }
            return value;
        }
    }
}

