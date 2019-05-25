using System.ComponentModel;
using System.Windows.Forms;

namespace GDMPlugins
{
    partial class DateDifferenceEditor
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
            this.OKbutton = new System.Windows.Forms.Button();
            this.groupingColumnsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.groupingColumnsLabel = new System.Windows.Forms.Label();
            this.tableComboBox = new System.Windows.Forms.ComboBox();
            this.tableLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(15, 204);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 0;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // groupingColumnsCheckedListBox
            // 
            this.groupingColumnsCheckedListBox.FormattingEnabled = true;
            this.groupingColumnsCheckedListBox.Location = new System.Drawing.Point(12, 95);
            this.groupingColumnsCheckedListBox.Name = "groupingColumnsCheckedListBox";
            this.groupingColumnsCheckedListBox.Size = new System.Drawing.Size(185, 94);
            this.groupingColumnsCheckedListBox.TabIndex = 1;
            // 
            // groupingColumnsLabel
            // 
            this.groupingColumnsLabel.AutoSize = true;
            this.groupingColumnsLabel.Location = new System.Drawing.Point(12, 76);
            this.groupingColumnsLabel.Name = "groupingColumnsLabel";
            this.groupingColumnsLabel.Size = new System.Drawing.Size(93, 13);
            this.groupingColumnsLabel.TabIndex = 2;
            this.groupingColumnsLabel.Text = "Grouping Columns";
            // 
            // tableComboBox
            // 
            this.tableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableComboBox.FormattingEnabled = true;
            this.tableComboBox.Location = new System.Drawing.Point(15, 36);
            this.tableComboBox.Name = "tableComboBox";
            this.tableComboBox.Size = new System.Drawing.Size(182, 21);
            this.tableComboBox.TabIndex = 3;
            this.tableComboBox.SelectedIndexChanged += new System.EventHandler(this.tableComboBox_SelectedIndexChanged);
            // 
            // tableLabel
            // 
            this.tableLabel.AutoSize = true;
            this.tableLabel.Location = new System.Drawing.Point(12, 20);
            this.tableLabel.Name = "tableLabel";
            this.tableLabel.Size = new System.Drawing.Size(34, 13);
            this.tableLabel.TabIndex = 4;
            this.tableLabel.Text = "Table";
            // 
            // DateDifferenceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 242);
            this.Controls.Add(this.tableLabel);
            this.Controls.Add(this.tableComboBox);
            this.Controls.Add(this.groupingColumnsLabel);
            this.Controls.Add(this.groupingColumnsCheckedListBox);
            this.Controls.Add(this.OKbutton);
            this.Name = "DateDifferenceEditor";
            this.Text = "Date Difference";
            this.Load += new System.EventHandler(this.DateDifferenceEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button OKbutton;
        private CheckedListBox groupingColumnsCheckedListBox;
        private Label groupingColumnsLabel;
        private ComboBox tableComboBox;
        private Label tableLabel;
    }
}