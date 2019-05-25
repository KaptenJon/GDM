using System.ComponentModel;
using System.Windows.Forms;

namespace GDMTool
{
    partial class Start
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            this.EditConfig = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.run = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.overallProgressBar = new System.Windows.Forms.ProgressBar();
            this.pluginProgressBar = new System.Windows.Forms.ProgressBar();
            this.overallProgress = new System.Windows.Forms.Label();
            this.pluginProgress = new System.Windows.Forms.Label();
            this.logfile = new System.Windows.Forms.CheckBox();
            this.logfilepath_label = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // EditConfig
            // 
            this.EditConfig.Image = ((System.Drawing.Image)(resources.GetObject("EditConfig.Image")));
            this.EditConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EditConfig.Location = new System.Drawing.Point(12, 12);
            this.EditConfig.Name = "EditConfig";
            this.EditConfig.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.EditConfig.Size = new System.Drawing.Size(111, 32);
            this.EditConfig.TabIndex = 2;
            this.EditConfig.Text = "   Edit Mode";
            this.EditConfig.UseVisualStyleBackColor = true;
            this.EditConfig.Click += new System.EventHandler(this.EditConfig_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(139, 12);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(138, 32);
            this.button1.TabIndex = 10;
            this.button1.Text = "   About GDM-Tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // run
            // 
            this.run.Enabled = false;
            this.run.Image = ((System.Drawing.Image)(resources.GetObject("run.Image")));
            this.run.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.run.Location = new System.Drawing.Point(13, 314);
            this.run.Name = "run";
            this.run.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.run.Size = new System.Drawing.Size(92, 68);
            this.run.TabIndex = 1;
            this.run.Text = " Run";
            this.run.UseVisualStyleBackColor = true;
            this.run.Click += new System.EventHandler(this.run_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(12, 72);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(186, 184);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(219, 72);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(263, 182);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.ClientSizeChanged += new System.EventHandler(this.flowLayoutPanel1_ClientSizeChanged);
            // 
            // overallProgressBar
            // 
            this.overallProgressBar.Enabled = false;
            this.overallProgressBar.Location = new System.Drawing.Point(126, 357);
            this.overallProgressBar.Name = "overallProgressBar";
            this.overallProgressBar.Size = new System.Drawing.Size(356, 24);
            this.overallProgressBar.TabIndex = 4;
            // 
            // pluginProgressBar
            // 
            this.pluginProgressBar.Enabled = false;
            this.pluginProgressBar.Location = new System.Drawing.Point(126, 314);
            this.pluginProgressBar.Name = "pluginProgressBar";
            this.pluginProgressBar.Size = new System.Drawing.Size(356, 14);
            this.pluginProgressBar.TabIndex = 5;
            // 
            // overallProgress
            // 
            this.overallProgress.AutoSize = true;
            this.overallProgress.Location = new System.Drawing.Point(123, 339);
            this.overallProgress.Name = "overallProgress";
            this.overallProgress.Size = new System.Drawing.Size(0, 13);
            this.overallProgress.TabIndex = 6;
            // 
            // pluginProgress
            // 
            this.pluginProgress.AutoSize = true;
            this.pluginProgress.Location = new System.Drawing.Point(123, 294);
            this.pluginProgress.Name = "pluginProgress";
            this.pluginProgress.Size = new System.Drawing.Size(0, 13);
            this.pluginProgress.TabIndex = 7;
            // 
            // logfile
            // 
            this.logfile.AutoSize = true;
            this.logfile.Enabled = false;
            this.logfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logfile.Location = new System.Drawing.Point(13, 269);
            this.logfile.Name = "logfile";
            this.logfile.Size = new System.Drawing.Size(68, 17);
            this.logfile.TabIndex = 8;
            this.logfile.Text = "Logfile:";
            this.logfile.UseVisualStyleBackColor = true;
            this.logfile.CheckedChanged += new System.EventHandler(this.logfile_CheckedChanged);
            // 
            // logfilepath_label
            // 
            this.logfilepath_label.AutoSize = true;
            this.logfilepath_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logfilepath_label.Location = new System.Drawing.Point(75, 271);
            this.logfilepath_label.Name = "logfilepath_label";
            this.logfilepath_label.Size = new System.Drawing.Size(0, 12);
            this.logfilepath_label.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(13, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(470, 1);
            this.panel2.TabIndex = 11;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 395);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logfilepath_label);
            this.Controls.Add(this.logfile);
            this.Controls.Add(this.pluginProgress);
            this.Controls.Add(this.overallProgress);
            this.Controls.Add(this.pluginProgressBar);
            this.Controls.Add(this.overallProgressBar);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.EditConfig);
            this.Controls.Add(this.run);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Start";
            this.Text = "Generic Data Management Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button EditConfig;
        private Button button1;
        private ListBox listBox1;
        private Button run;
        private Label logfilepath_label;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox logfile;
        private ProgressBar overallProgressBar;
        private Label pluginProgress;
        private ProgressBar pluginProgressBar;
        private Label overallProgress;
        private Panel panel2;
    }
}