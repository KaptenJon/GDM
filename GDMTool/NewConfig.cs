using System;
using System.Windows.Forms;

namespace GDMTool
{
    /// <summary>
    /// Form that is used to let the user input a name for a new Config.
    /// </summary>
    public partial class NewConfig : Form
    {
        public NewConfig()
        {
            InitializeComponent();
        }

        public string ConfigName => textBox1.Text;

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
