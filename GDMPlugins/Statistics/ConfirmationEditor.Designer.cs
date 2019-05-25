using System.ComponentModel;
using System.Windows.Forms;
using ZedGraphControl = ZedGraph.ZedGraphControl;

namespace GDMPlugins.Statistics
{
    partial class ConfirmationEditor
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
            this.acceptButton = new System.Windows.Forms.Button();
            this.groupingLabel = new System.Windows.Forms.Label();
            this.groupingLabel2 = new System.Windows.Forms.Label();
            this.ppPlotZedGraphControl = new ZedGraph.ZedGraphControl();
            this.distributionComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.acceptButton.Location = new System.Drawing.Point(26, 285);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // groupingLabel
            // 
            this.groupingLabel.AutoSize = true;
            this.groupingLabel.Location = new System.Drawing.Point(23, 16);
            this.groupingLabel.Name = "groupingLabel";
            this.groupingLabel.Size = new System.Drawing.Size(53, 13);
            this.groupingLabel.TabIndex = 1;
            this.groupingLabel.Text = "Grouping:";
            // 
            // groupingLabel2
            // 
            this.groupingLabel2.AutoSize = true;
            this.groupingLabel2.Location = new System.Drawing.Point(82, 16);
            this.groupingLabel2.Name = "groupingLabel2";
            this.groupingLabel2.Size = new System.Drawing.Size(35, 13);
            this.groupingLabel2.TabIndex = 2;
            this.groupingLabel2.Text = "label1";
            // 
            // ppPlotZedGraphControl
            // 
            this.ppPlotZedGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ppPlotZedGraphControl.Location = new System.Drawing.Point(26, 40);
            this.ppPlotZedGraphControl.Name = "ppPlotZedGraphControl";
            this.ppPlotZedGraphControl.ScrollGrace = 0;
            this.ppPlotZedGraphControl.ScrollMaxX = 0;
            this.ppPlotZedGraphControl.ScrollMaxY = 0;
            this.ppPlotZedGraphControl.ScrollMaxY2 = 0;
            this.ppPlotZedGraphControl.ScrollMinX = 0;
            this.ppPlotZedGraphControl.ScrollMinY = 0;
            this.ppPlotZedGraphControl.ScrollMinY2 = 0;
            this.ppPlotZedGraphControl.Size = new System.Drawing.Size(511, 227);
            this.ppPlotZedGraphControl.TabIndex = 14;
            // 
            // distributionComboBox
            // 
            this.distributionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.distributionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.distributionComboBox.FormattingEnabled = true;
            this.distributionComboBox.Location = new System.Drawing.Point(411, 13);
            this.distributionComboBox.Name = "distributionComboBox";
            this.distributionComboBox.Size = new System.Drawing.Size(126, 21);
            this.distributionComboBox.TabIndex = 17;
            this.distributionComboBox.SelectedIndexChanged += new System.EventHandler(this.DistrubutionComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(346, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Distribution";
            // 
            // ManualConfirmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 320);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.distributionComboBox);
            this.Controls.Add(this.ppPlotZedGraphControl);
            this.Controls.Add(this.groupingLabel2);
            this.Controls.Add(this.groupingLabel);
            this.Controls.Add(this.acceptButton);
            this.Name = "ManualConfirmation";
            this.Text = "Manual Confirmation";
            this.Load += new System.EventHandler(this.Confirmation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button acceptButton;
        private Label groupingLabel;
        private Label groupingLabel2;
        private ZedGraphControl ppPlotZedGraphControl;
        private ComboBox distributionComboBox;
        private Label label7;
    }
}