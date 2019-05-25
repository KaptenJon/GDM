using System.ComponentModel;
using System.Windows.Forms;

namespace GDMTool
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.overalllable = new System.Windows.Forms.ToolStripStatusLabel();
            this.overallprogressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.OperationLable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TableBatch = new System.Windows.Forms.Button();
            this.Batch = new System.Windows.Forms.Button();
            this.showAll = new System.Windows.Forms.CheckBox();
            this.pluginMenu = new System.Windows.Forms.MenuStrip();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.logBtn = new System.Windows.Forms.Button();
            this.applyBtn = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.configMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.newConfig = new System.Windows.Forms.ToolStripButton();
            this.openConfig = new System.Windows.Forms.ToolStripButton();
            this.mergeConfig = new System.Windows.Forms.ToolStripButton();
            this.saveConfig = new System.Windows.Forms.ToolStripButton();
            this.deleteConfig = new System.Windows.Forms.ToolStripButton();
            this.importConfig = new System.Windows.Forms.ToolStripButton();
            this.exportConfig = new System.Windows.Forms.ToolStripButton();
            this.Configure = new System.Windows.Forms.ToolStripButton();
            this.treeView = new System.Windows.Forms.TreeView();
            this.mnuExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.configMenu.SuspendLayout();
            this.mnuExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overalllable,
            this.overallprogressbar,
            this.OperationLable,
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 592);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1212, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // overalllable
            // 
            this.overalllable.Name = "overalllable";
            this.overalllable.Size = new System.Drawing.Size(79, 17);
            this.overalllable.Text = "Overall Status";
            this.overalllable.Visible = false;
            // 
            // overallprogressbar
            // 
            this.overallprogressbar.Name = "overallprogressbar";
            this.overallprogressbar.Size = new System.Drawing.Size(100, 16);
            // 
            // OperationLable
            // 
            this.OperationLable.Name = "OperationLable";
            this.OperationLable.Size = new System.Drawing.Size(95, 17);
            this.OperationLable.Text = "Operation Status";
            this.OperationLable.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.TableBatch);
            this.splitContainer1.Panel1.Controls.Add(this.Batch);
            this.splitContainer1.Panel1.Controls.Add(this.showAll);
            this.splitContainer1.Panel1.Controls.Add(this.pluginMenu);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1212, 592);
            this.splitContainer1.SplitterDistance = 268;
            this.splitContainer1.TabIndex = 4;
            // 
            // TableBatch
            // 
            this.TableBatch.Location = new System.Drawing.Point(3, 31);
            this.TableBatch.Name = "TableBatch";
            this.TableBatch.Size = new System.Drawing.Size(121, 23);
            this.TableBatch.TabIndex = 5;
            this.TableBatch.Text = "Start new table batch";
            this.TableBatch.UseVisualStyleBackColor = true;
            this.TableBatch.Click += new System.EventHandler(this.TableBatch_Click);
            // 
            // Batch
            // 
            this.Batch.Location = new System.Drawing.Point(3, 2);
            this.Batch.Name = "Batch";
            this.Batch.Size = new System.Drawing.Size(121, 23);
            this.Batch.TabIndex = 4;
            this.Batch.Text = "Start new file batch";
            this.Batch.UseVisualStyleBackColor = true;
            this.Batch.Click += new System.EventHandler(this.Batch_Click);
            // 
            // showAll
            // 
            this.showAll.AutoSize = true;
            this.showAll.Location = new System.Drawing.Point(3, 60);
            this.showAll.Name = "showAll";
            this.showAll.Size = new System.Drawing.Size(102, 17);
            this.showAll.TabIndex = 3;
            this.showAll.Text = "Show all plugins";
            this.showAll.UseVisualStyleBackColor = true;
            this.showAll.CheckedChanged += new System.EventHandler(this.showAll_CheckedChanged);
            // 
            // pluginMenu
            // 
            this.pluginMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.pluginMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.pluginMenu.Location = new System.Drawing.Point(0, 80);
            this.pluginMenu.Name = "pluginMenu";
            this.pluginMenu.ShowItemToolTips = true;
            this.pluginMenu.Size = new System.Drawing.Size(30, 206);
            this.pluginMenu.TabIndex = 2;
            this.pluginMenu.Text = "pluginMenu";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.configMenu);
            this.splitContainer2.Panel2.Controls.Add(this.treeView);
            this.splitContainer2.Size = new System.Drawing.Size(940, 592);
            this.splitContainer2.SplitterDistance = 639;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer3.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel2.Controls.Add(this.logBtn);
            this.splitContainer3.Panel2.Controls.Add(this.applyBtn);
            this.splitContainer3.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer3.Size = new System.Drawing.Size(639, 592);
            this.splitContainer3.SplitterDistance = 408;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.HotTrack = true;
            this.tabControl.Location = new System.Drawing.Point(6, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 395);
            this.tabControl.TabIndex = 1;
            // 
            // logBtn
            // 
            this.logBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logBtn.Location = new System.Drawing.Point(564, 44);
            this.logBtn.Name = "logBtn";
            this.logBtn.Size = new System.Drawing.Size(57, 23);
            this.logBtn.TabIndex = 6;
            this.logBtn.Text = "Log";
            this.logBtn.UseVisualStyleBackColor = true;
            // 
            // applyBtn
            // 
            this.applyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyBtn.Location = new System.Drawing.Point(564, 15);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(57, 23);
            this.applyBtn.TabIndex = 4;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid.Location = new System.Drawing.Point(-1, -1);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(552, 179);
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // configMenu
            // 
            this.configMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.configMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.newConfig,
            this.openConfig,
            this.mergeConfig,
            this.saveConfig,
            this.deleteConfig,
            this.importConfig,
            this.exportConfig,
            this.Configure});
            this.configMenu.Location = new System.Drawing.Point(0, 0);
            this.configMenu.Name = "configMenu";
            this.configMenu.Size = new System.Drawing.Size(293, 25);
            this.configMenu.TabIndex = 2;
            this.configMenu.Text = "toolStrip1";
            this.configMenu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.configMenu_KeyDown);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "Configurations";
            // 
            // newConfig
            // 
            this.newConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newConfig.Image = ((System.Drawing.Image)(resources.GetObject("newConfig.Image")));
            this.newConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newConfig.Name = "newConfig";
            this.newConfig.Size = new System.Drawing.Size(23, 22);
            this.newConfig.Text = "&New Data Configuration";
            this.newConfig.Click += new System.EventHandler(this.newConfig_Click);
            // 
            // openConfig
            // 
            this.openConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openConfig.Enabled = false;
            this.openConfig.Image = ((System.Drawing.Image)(resources.GetObject("openConfig.Image")));
            this.openConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openConfig.Name = "openConfig";
            this.openConfig.Size = new System.Drawing.Size(23, 22);
            this.openConfig.Text = "&Load Saved Data Configuration";
            // 
            // mergeConfig
            // 
            this.mergeConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mergeConfig.Image = ((System.Drawing.Image)(resources.GetObject("mergeConfig.Image")));
            this.mergeConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mergeConfig.Name = "mergeConfig";
            this.mergeConfig.Size = new System.Drawing.Size(23, 22);
            this.mergeConfig.Text = "Add Configuration";
            // 
            // saveConfig
            // 
            this.saveConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveConfig.Enabled = false;
            this.saveConfig.Image = ((System.Drawing.Image)(resources.GetObject("saveConfig.Image")));
            this.saveConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveConfig.Name = "saveConfig";
            this.saveConfig.Size = new System.Drawing.Size(23, 22);
            this.saveConfig.Text = "&Save Current Data Configuration";
            // 
            // deleteConfig
            // 
            this.deleteConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteConfig.Enabled = false;
            this.deleteConfig.Image = ((System.Drawing.Image)(resources.GetObject("deleteConfig.Image")));
            this.deleteConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteConfig.Name = "deleteConfig";
            this.deleteConfig.Size = new System.Drawing.Size(23, 22);
            this.deleteConfig.Text = "Delete Saved Configuration";
            // 
            // importConfig
            // 
            this.importConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importConfig.Image = ((System.Drawing.Image)(resources.GetObject("importConfig.Image")));
            this.importConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importConfig.Name = "importConfig";
            this.importConfig.Size = new System.Drawing.Size(23, 22);
            this.importConfig.Text = "Import Configuration";
            // 
            // exportConfig
            // 
            this.exportConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportConfig.Enabled = false;
            this.exportConfig.Image = ((System.Drawing.Image)(resources.GetObject("exportConfig.Image")));
            this.exportConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportConfig.Name = "exportConfig";
            this.exportConfig.Size = new System.Drawing.Size(23, 22);
            this.exportConfig.Text = "Export Saved Configuration";
            // 
            // Configure
            // 
            this.Configure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Configure.Image = ((System.Drawing.Image)(resources.GetObject("Configure.Image")));
            this.Configure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Configure.Name = "Configure";
            this.Configure.Size = new System.Drawing.Size(23, 22);
            this.Configure.Text = "Configurate GDM Service Path";
            this.Configure.Click += new System.EventHandler(this.Configure_Click);
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BackColor = System.Drawing.SystemColors.Control;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Location = new System.Drawing.Point(0, 42);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(293, 546);
            this.treeView.TabIndex = 1;
            this.treeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.configMenu_KeyDown);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp);
            // 
            // mnuExport
            // 
            this.mnuExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.removeToolStripMenuItem});
            this.mnuExport.Name = "mnuTextFile";
            this.mnuExport.Size = new System.Drawing.Size(221, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItem1.Text = "Export to GDM Daily Service";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 614);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Generic Data Management Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.configMenu.ResumeLayout(false);
            this.configMenu.PerformLayout();
            this.mnuExport.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip;
        private SplitContainer splitContainer1;
        private MenuStrip pluginMenu;
        private SplitContainer splitContainer2;
        private PropertyGrid propertyGrid;
        private Button applyBtn;
        private SplitContainer splitContainer3;
        private TabControl tabControl;
        private TreeView treeView;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStrip configMenu;
        private ToolStripButton newConfig;
        private ToolStripButton openConfig;
        private ToolStripButton saveConfig;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton deleteConfig;
        private ToolStripButton importConfig;
        private ToolStripButton exportConfig;
        private Button logBtn;
        private ContextMenuStrip mnuExport;
        private ToolStripMenuItem toolStripMenuItem1;
        private CheckBox showAll;
        private ToolStripMenuItem removeToolStripMenuItem;
        private Button Batch;
        private Button TableBatch;
        private ToolStripButton mergeConfig;
        private ToolStripButton Configure;
        private ToolStripStatusLabel OperationLable;
        private ToolStripStatusLabel overalllable;
        private ToolStripProgressBar overallprogressbar;
    }
}

