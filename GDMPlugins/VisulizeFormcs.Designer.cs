namespace GDMPlugins
{
    partial class VisulizeFormcs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.ResourceActiveTimeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.visulizationDataset = new GDMPlugins.VisulizationDataset();
            this.report = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceActiveTimeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visulizationDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // ResourceActiveTimeBindingSource
            // 
            this.ResourceActiveTimeBindingSource.DataMember = "ResourceActiveTime";
            this.ResourceActiveTimeBindingSource.DataSource = this.visulizationDataset;
            // 
            // visulizationDataset
            // 
            this.visulizationDataset.DataSetName = "VisulizationDataset";
            this.visulizationDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // report
            // 
            this.report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.report.AutoSize = true;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ResourceActiveTimeBindingSource;
            this.report.LocalReport.DataSources.Add(reportDataSource1);
            this.report.LocalReport.ReportEmbeddedResource = "GDMPlugins.Report1.rdlc";
            this.report.Location = new System.Drawing.Point(7, 7);
            this.report.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.report.Name = "report";
            this.report.Size = new System.Drawing.Size(985, 564);
            this.report.TabIndex = 0;
            // 
            // VisulizeFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 571);
            this.Controls.Add(this.report);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VisulizeFormcs";
            this.Text = "VisulizeFormcs";
            this.Load += new System.EventHandler(this.VisulizeFormcs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResourceActiveTimeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visulizationDataset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer report;
        private VisulizationDataset visulizationDataset;
        private System.Windows.Forms.BindingSource ResourceActiveTimeBindingSource;
    }
}