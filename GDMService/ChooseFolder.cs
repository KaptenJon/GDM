using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMService
{
    public partial class ChooseFolder : Form
    {
        public string Path { get; private set; }
        public ChooseFolder()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            Path = folderBrowserDialog1.SelectedPath;
        }

        private void ok_Click(object sender, EventArgs e)
        {
           Close();
        }
    }
}
