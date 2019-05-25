using System.ComponentModel;
using System.Windows.Forms;

namespace GDMTool
{
    partial class LogViewer
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
            this.closeButton = new System.Windows.Forms.Button();
            this.chapterComboBox = new System.Windows.Forms.ComboBox();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.displayLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.closeButton.Location = new System.Drawing.Point(12, 202);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // chapterComboBox
            // 
            this.chapterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chapterComboBox.FormattingEnabled = true;
            this.chapterComboBox.Location = new System.Drawing.Point(12, 31);
            this.chapterComboBox.Name = "chapterComboBox";
            this.chapterComboBox.Size = new System.Drawing.Size(175, 21);
            this.chapterComboBox.TabIndex = 2;
            this.chapterComboBox.SelectedIndexChanged += new System.EventHandler(this.ChapterComboBox_SelectedIndexChanged);
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.logRichTextBox.Location = new System.Drawing.Point(211, 12);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.Size = new System.Drawing.Size(385, 213);
            this.logRichTextBox.TabIndex = 3;
            this.logRichTextBox.Text = "";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(112, 202);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Location = new System.Drawing.Point(13, 12);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(41, 13);
            this.displayLabel.TabIndex = 5;
            this.displayLabel.Text = "Display";
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(608, 237);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.logRichTextBox);
            this.Controls.Add(this.chapterComboBox);
            this.Controls.Add(this.closeButton);
            this.Name = "LogViewer";
            this.Text = "Log Viewer";
            this.Resize += new System.EventHandler(this.LogViewer_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button closeButton;
        private ComboBox chapterComboBox;
        private RichTextBox logRichTextBox;
        private Button saveButton;
        private Label displayLabel;
    }
}