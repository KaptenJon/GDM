using System.ComponentModel;
using System.Windows.Forms;

namespace GDMPlugins
{
    partial class DateIntervalEditor
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
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.filterModeComboBox = new System.Windows.Forms.ComboBox();
            this.filterModeLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.lowerBoundTextBox = new System.Windows.Forms.TextBox();
            this.upperBoundTextBox = new System.Windows.Forms.TextBox();
            this.lowerBoundLabel = new System.Windows.Forms.Label();
            this.upperBoundLabel = new System.Windows.Forms.Label();
            this.lowerBoundRadioButton = new System.Windows.Forms.RadioButton();
            this.upperBoundRadioButton = new System.Windows.Forms.RadioButton();
            this.milliSecondsTextBox = new System.Windows.Forms.TextBox();
            this.milliSecondsLabel = new System.Windows.Forms.Label();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.minuteComboBox = new System.Windows.Forms.ComboBox();
            this.hourLabel = new System.Windows.Forms.Label();
            this.minuteLabel = new System.Windows.Forms.Label();
            this.secondsComboBox = new System.Windows.Forms.ComboBox();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.RelativeAbsoluteCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.days = new System.Windows.Forms.ComboBox();
            this.year = new System.Windows.Forms.Label();
            this.years = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(28, 81);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            // 
            // filterModeComboBox
            // 
            this.filterModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterModeComboBox.FormattingEnabled = true;
            this.filterModeComboBox.Location = new System.Drawing.Point(28, 34);
            this.filterModeComboBox.Name = "filterModeComboBox";
            this.filterModeComboBox.Size = new System.Drawing.Size(144, 21);
            this.filterModeComboBox.TabIndex = 1;
            // 
            // filterModeLabel
            // 
            this.filterModeLabel.AutoSize = true;
            this.filterModeLabel.Location = new System.Drawing.Point(25, 18);
            this.filterModeLabel.Name = "filterModeLabel";
            this.filterModeLabel.Size = new System.Drawing.Size(29, 13);
            this.filterModeLabel.TabIndex = 2;
            this.filterModeLabel.Text = "Filter";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(28, 287);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(28, 333);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 4;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // lowerBoundTextBox
            // 
            this.lowerBoundTextBox.Location = new System.Drawing.Point(226, 287);
            this.lowerBoundTextBox.Name = "lowerBoundTextBox";
            this.lowerBoundTextBox.Size = new System.Drawing.Size(173, 20);
            this.lowerBoundTextBox.TabIndex = 6;
            // 
            // upperBoundTextBox
            // 
            this.upperBoundTextBox.Location = new System.Drawing.Point(226, 340);
            this.upperBoundTextBox.Name = "upperBoundTextBox";
            this.upperBoundTextBox.Size = new System.Drawing.Size(173, 20);
            this.upperBoundTextBox.TabIndex = 7;
            // 
            // lowerBoundLabel
            // 
            this.lowerBoundLabel.AutoSize = true;
            this.lowerBoundLabel.Location = new System.Drawing.Point(223, 271);
            this.lowerBoundLabel.Name = "lowerBoundLabel";
            this.lowerBoundLabel.Size = new System.Drawing.Size(30, 13);
            this.lowerBoundLabel.TabIndex = 8;
            this.lowerBoundLabel.Text = "From";
            // 
            // upperBoundLabel
            // 
            this.upperBoundLabel.AutoSize = true;
            this.upperBoundLabel.Location = new System.Drawing.Point(223, 324);
            this.upperBoundLabel.Name = "upperBoundLabel";
            this.upperBoundLabel.Size = new System.Drawing.Size(20, 13);
            this.upperBoundLabel.TabIndex = 9;
            this.upperBoundLabel.Text = "To";
            // 
            // lowerBoundRadioButton
            // 
            this.lowerBoundRadioButton.AutoSize = true;
            this.lowerBoundRadioButton.Checked = true;
            this.lowerBoundRadioButton.Location = new System.Drawing.Point(196, 290);
            this.lowerBoundRadioButton.Name = "lowerBoundRadioButton";
            this.lowerBoundRadioButton.Size = new System.Drawing.Size(14, 13);
            this.lowerBoundRadioButton.TabIndex = 10;
            this.lowerBoundRadioButton.TabStop = true;
            this.lowerBoundRadioButton.UseVisualStyleBackColor = true;
            // 
            // upperBoundRadioButton
            // 
            this.upperBoundRadioButton.AutoSize = true;
            this.upperBoundRadioButton.Location = new System.Drawing.Point(196, 343);
            this.upperBoundRadioButton.Name = "upperBoundRadioButton";
            this.upperBoundRadioButton.Size = new System.Drawing.Size(14, 13);
            this.upperBoundRadioButton.TabIndex = 11;
            this.upperBoundRadioButton.TabStop = true;
            this.upperBoundRadioButton.UseVisualStyleBackColor = true;
            // 
            // milliSecondsTextBox
            // 
            this.milliSecondsTextBox.Location = new System.Drawing.Point(262, 177);
            this.milliSecondsTextBox.Name = "milliSecondsTextBox";
            this.milliSecondsTextBox.Size = new System.Drawing.Size(63, 20);
            this.milliSecondsTextBox.TabIndex = 12;
            this.milliSecondsTextBox.Text = "000";
            // 
            // milliSecondsLabel
            // 
            this.milliSecondsLabel.AutoSize = true;
            this.milliSecondsLabel.Location = new System.Drawing.Point(259, 161);
            this.milliSecondsLabel.Name = "milliSecondsLabel";
            this.milliSecondsLabel.Size = new System.Drawing.Size(66, 13);
            this.milliSecondsLabel.TabIndex = 13;
            this.milliSecondsLabel.Text = "MilliSeconds";
            // 
            // hourComboBox
            // 
            this.hourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Location = new System.Drawing.Point(262, 122);
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(40, 21);
            this.hourComboBox.TabIndex = 14;
            // 
            // minuteComboBox
            // 
            this.minuteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minuteComboBox.FormattingEnabled = true;
            this.minuteComboBox.Location = new System.Drawing.Point(329, 122);
            this.minuteComboBox.Name = "minuteComboBox";
            this.minuteComboBox.Size = new System.Drawing.Size(40, 21);
            this.minuteComboBox.TabIndex = 15;
            // 
            // hourLabel
            // 
            this.hourLabel.AutoSize = true;
            this.hourLabel.Location = new System.Drawing.Point(262, 103);
            this.hourLabel.Name = "hourLabel";
            this.hourLabel.Size = new System.Drawing.Size(30, 13);
            this.hourLabel.TabIndex = 16;
            this.hourLabel.Text = "Hour";
            // 
            // minuteLabel
            // 
            this.minuteLabel.AutoSize = true;
            this.minuteLabel.Location = new System.Drawing.Point(330, 103);
            this.minuteLabel.Name = "minuteLabel";
            this.minuteLabel.Size = new System.Drawing.Size(39, 13);
            this.minuteLabel.TabIndex = 17;
            this.minuteLabel.Text = "Minute";
            // 
            // secondsComboBox
            // 
            this.secondsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.secondsComboBox.FormattingEnabled = true;
            this.secondsComboBox.Location = new System.Drawing.Point(395, 122);
            this.secondsComboBox.Name = "secondsComboBox";
            this.secondsComboBox.Size = new System.Drawing.Size(40, 21);
            this.secondsComboBox.TabIndex = 18;
            // 
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(392, 103);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(49, 13);
            this.secondsLabel.TabIndex = 19;
            this.secondsLabel.Text = "Seconds";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(360, 175);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 20;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // RelativeAbsoluteCombo
            // 
            this.RelativeAbsoluteCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RelativeAbsoluteCombo.FormattingEnabled = true;
            this.RelativeAbsoluteCombo.Items.AddRange(new object[] {
            "Relative Date",
            "Absolute Date"});
            this.RelativeAbsoluteCombo.Location = new System.Drawing.Point(196, 34);
            this.RelativeAbsoluteCombo.Name = "RelativeAbsoluteCombo";
            this.RelativeAbsoluteCombo.Size = new System.Drawing.Size(144, 21);
            this.RelativeAbsoluteCombo.TabIndex = 21;
            this.RelativeAbsoluteCombo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Relative";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Days";
            // 
            // days
            // 
            this.days.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.days.FormattingEnabled = true;
            this.days.Location = new System.Drawing.Point(122, 122);
            this.days.Name = "days";
            this.days.Size = new System.Drawing.Size(40, 21);
            this.days.TabIndex = 27;
            // 
            // year
            // 
            this.year.AutoSize = true;
            this.year.Location = new System.Drawing.Point(54, 103);
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(34, 13);
            this.year.TabIndex = 25;
            this.year.Text = "Years";
            // 
            // years
            // 
            this.years.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.years.FormattingEnabled = true;
            this.years.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.years.Location = new System.Drawing.Point(54, 122);
            this.years.Name = "years";
            this.years.Size = new System.Drawing.Size(40, 21);
            this.years.TabIndex = 23;
            // 
            // DateIntervalEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 382);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.days);
            this.Controls.Add(this.year);
            this.Controls.Add(this.years);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RelativeAbsoluteCombo);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.secondsLabel);
            this.Controls.Add(this.secondsComboBox);
            this.Controls.Add(this.minuteLabel);
            this.Controls.Add(this.hourLabel);
            this.Controls.Add(this.minuteComboBox);
            this.Controls.Add(this.hourComboBox);
            this.Controls.Add(this.milliSecondsLabel);
            this.Controls.Add(this.milliSecondsTextBox);
            this.Controls.Add(this.upperBoundRadioButton);
            this.Controls.Add(this.lowerBoundRadioButton);
            this.Controls.Add(this.upperBoundLabel);
            this.Controls.Add(this.lowerBoundLabel);
            this.Controls.Add(this.upperBoundTextBox);
            this.Controls.Add(this.lowerBoundTextBox);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.filterModeLabel);
            this.Controls.Add(this.filterModeComboBox);
            this.Name = "DateIntervalEditor";
            this.Text = "Date interval filter";
            this.Load += new System.EventHandler(this.DateIntervalEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MonthCalendar monthCalendar;
        private ComboBox filterModeComboBox;
        private Label filterModeLabel;
        private Button okButton;
        private Button clearButton;
        private TextBox lowerBoundTextBox;
        private TextBox upperBoundTextBox;
        private Label lowerBoundLabel;
        private Label upperBoundLabel;
        private RadioButton lowerBoundRadioButton;
        private RadioButton upperBoundRadioButton;
        private TextBox milliSecondsTextBox;
        private Label milliSecondsLabel;
        private ComboBox hourComboBox;
        private ComboBox minuteComboBox;
        private Label hourLabel;
        private Label minuteLabel;
        private ComboBox secondsComboBox;
        private Label secondsLabel;
        private Button addButton;
        private ComboBox RelativeAbsoluteCombo;
        private Label label1;
        private Label label2;
        private ComboBox days;
        private Label year;
        private ComboBox years;
    }
}