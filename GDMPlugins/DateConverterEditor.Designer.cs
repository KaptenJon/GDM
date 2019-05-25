using System.ComponentModel;
using System.Windows.Forms;

namespace GDMPlugins
{
    partial class DateConverterEditor
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
            this.listBox = new System.Windows.Forms.ListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.dtFormatPatternLabel = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.dtAtomsLabel = new System.Windows.Forms.Label();
            this.specifiedPatternsLabel = new System.Windows.Forms.Label();
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(200, 122);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(174, 80);
            this.listBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(200, 219);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(299, 219);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // dtFormatPatternLabel
            // 
            this.dtFormatPatternLabel.AutoSize = true;
            this.dtFormatPatternLabel.Location = new System.Drawing.Point(249, 19);
            this.dtFormatPatternLabel.Name = "dtFormatPatternLabel";
            this.dtFormatPatternLabel.Size = new System.Drawing.Size(125, 13);
            this.dtFormatPatternLabel.TabIndex = 4;
            this.dtFormatPatternLabel.Text = "DateTime Format Pattern";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(225, 35);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(149, 20);
            this.textBox.TabIndex = 5;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(299, 61);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // dtAtomsLabel
            // 
            this.dtAtomsLabel.AutoSize = true;
            this.dtAtomsLabel.Location = new System.Drawing.Point(20, 19);
            this.dtAtomsLabel.Name = "dtAtomsLabel";
            this.dtAtomsLabel.Size = new System.Drawing.Size(84, 13);
            this.dtAtomsLabel.TabIndex = 10;
            this.dtAtomsLabel.Text = "DateTime atoms";
            // 
            // specifiedPatternsLabel
            // 
            this.specifiedPatternsLabel.AutoSize = true;
            this.specifiedPatternsLabel.Location = new System.Drawing.Point(282, 106);
            this.specifiedPatternsLabel.Name = "specifiedPatternsLabel";
            this.specifiedPatternsLabel.Size = new System.Drawing.Size(92, 13);
            this.specifiedPatternsLabel.TabIndex = 11;
            this.specifiedPatternsLabel.Text = "Specified patterns";
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(23, 35);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(132, 207);
            this.treeView.TabIndex = 13;
            // 
            // DateConverterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 260);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.specifiedPatternsLabel);
            this.Controls.Add(this.dtAtomsLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.dtFormatPatternLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.listBox);
            this.Name = "DateConverterEditor";
            this.Text = "Date Converter";
            this.Load += new System.EventHandler(this.DateConverterEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBox;
        private Button okButton;
        private Button clearButton;
        private Label dtFormatPatternLabel;
        private TextBox textBox;
        private Button addButton;
        private Label dtAtomsLabel;
        private Label specifiedPatternsLabel;
        private TreeView treeView;
    }
}