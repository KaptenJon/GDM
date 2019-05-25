using System.ComponentModel;
using System.Windows.Forms;
using ZedGraph;
using Label = System.Windows.Forms.Label;

namespace GDMPlugins.Statistics
{
    partial class GroupingAndPreviewEditor
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rSquaresListBox = new System.Windows.Forms.ListBox();
            this.rSquaresLabel = new System.Windows.Forms.Label();
            this.sampleSizeResultLabel = new System.Windows.Forms.Label();
            this.sampleSizeLabel = new System.Windows.Forms.Label();
            this.skewnessResultLabel = new System.Windows.Forms.Label();
            this.stdDeviationResultLabel = new System.Windows.Forms.Label();
            this.varianceResultLabel = new System.Windows.Forms.Label();
            this.meanResultLabel = new System.Windows.Forms.Label();
            this.minimumResultLabel = new System.Windows.Forms.Label();
            this.maximumResultLabel = new System.Windows.Forms.Label();
            this.skewnessLabel = new System.Windows.Forms.Label();
            this.stdDeviationLabel = new System.Windows.Forms.Label();
            this.varianceLabel = new System.Windows.Forms.Label();
            this.meanLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.maxLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.intervalsLabel = new System.Windows.Forms.Label();
            this.intervalsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.histogramZedGraphControl = new ZedGraph.ZedGraphControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.scatterPlotZedGraphControl = new ZedGraph.ZedGraphControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.distrubutionComboBox = new System.Windows.Forms.ComboBox();
            this.ppPlotZedGraphControl = new ZedGraph.ZedGraphControl();
            this.label2 = new System.Windows.Forms.Label();
            this.tableComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupingColumnsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataColumnComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.previewGroupComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervalsNumericUpDown)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Location = new System.Drawing.Point(228, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(487, 263);
            this.tabControl.TabIndex = 3;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rSquaresListBox);
            this.tabPage1.Controls.Add(this.rSquaresLabel);
            this.tabPage1.Controls.Add(this.sampleSizeResultLabel);
            this.tabPage1.Controls.Add(this.sampleSizeLabel);
            this.tabPage1.Controls.Add(this.skewnessResultLabel);
            this.tabPage1.Controls.Add(this.stdDeviationResultLabel);
            this.tabPage1.Controls.Add(this.varianceResultLabel);
            this.tabPage1.Controls.Add(this.meanResultLabel);
            this.tabPage1.Controls.Add(this.minimumResultLabel);
            this.tabPage1.Controls.Add(this.maximumResultLabel);
            this.tabPage1.Controls.Add(this.skewnessLabel);
            this.tabPage1.Controls.Add(this.stdDeviationLabel);
            this.tabPage1.Controls.Add(this.varianceLabel);
            this.tabPage1.Controls.Add(this.meanLabel);
            this.tabPage1.Controls.Add(this.minimumLabel);
            this.tabPage1.Controls.Add(this.maxLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(479, 237);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Statistical Summary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rSquaresListBox
            // 
            this.rSquaresListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rSquaresListBox.FormattingEnabled = true;
            this.rSquaresListBox.Location = new System.Drawing.Point(165, 65);
            this.rSquaresListBox.Name = "rSquaresListBox";
            this.rSquaresListBox.Size = new System.Drawing.Size(285, 147);
            this.rSquaresListBox.TabIndex = 20;
            // 
            // rSquaresLabel
            // 
            this.rSquaresLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rSquaresLabel.AutoSize = true;
            this.rSquaresLabel.Location = new System.Drawing.Point(400, 43);
            this.rSquaresLabel.Name = "rSquaresLabel";
            this.rSquaresLabel.Size = new System.Drawing.Size(52, 13);
            this.rSquaresLabel.TabIndex = 19;
            this.rSquaresLabel.Text = "R Square";
            // 
            // sampleSizeResultLabel
            // 
            this.sampleSizeResultLabel.AutoSize = true;
            this.sampleSizeResultLabel.Location = new System.Drawing.Point(120, 171);
            this.sampleSizeResultLabel.Name = "sampleSizeResultLabel";
            this.sampleSizeResultLabel.Size = new System.Drawing.Size(13, 13);
            this.sampleSizeResultLabel.TabIndex = 18;
            this.sampleSizeResultLabel.Text = "0";
            // 
            // sampleSizeLabel
            // 
            this.sampleSizeLabel.AutoSize = true;
            this.sampleSizeLabel.Location = new System.Drawing.Point(16, 171);
            this.sampleSizeLabel.Name = "sampleSizeLabel";
            this.sampleSizeLabel.Size = new System.Drawing.Size(63, 13);
            this.sampleSizeLabel.TabIndex = 17;
            this.sampleSizeLabel.Text = "Sample size";
            // 
            // skewnessResultLabel
            // 
            this.skewnessResultLabel.AutoSize = true;
            this.skewnessResultLabel.Location = new System.Drawing.Point(120, 147);
            this.skewnessResultLabel.Name = "skewnessResultLabel";
            this.skewnessResultLabel.Size = new System.Drawing.Size(22, 13);
            this.skewnessResultLabel.TabIndex = 16;
            this.skewnessResultLabel.Text = "0.0";
            // 
            // stdDeviationResultLabel
            // 
            this.stdDeviationResultLabel.AutoSize = true;
            this.stdDeviationResultLabel.Location = new System.Drawing.Point(120, 123);
            this.stdDeviationResultLabel.Name = "stdDeviationResultLabel";
            this.stdDeviationResultLabel.Size = new System.Drawing.Size(22, 13);
            this.stdDeviationResultLabel.TabIndex = 15;
            this.stdDeviationResultLabel.Text = "0.0";
            // 
            // varianceResultLabel
            // 
            this.varianceResultLabel.AutoSize = true;
            this.varianceResultLabel.Location = new System.Drawing.Point(120, 99);
            this.varianceResultLabel.Name = "varianceResultLabel";
            this.varianceResultLabel.Size = new System.Drawing.Size(22, 13);
            this.varianceResultLabel.TabIndex = 14;
            this.varianceResultLabel.Text = "0.0";
            // 
            // meanResultLabel
            // 
            this.meanResultLabel.AutoSize = true;
            this.meanResultLabel.Location = new System.Drawing.Point(120, 75);
            this.meanResultLabel.Name = "meanResultLabel";
            this.meanResultLabel.Size = new System.Drawing.Size(22, 13);
            this.meanResultLabel.TabIndex = 13;
            this.meanResultLabel.Text = "0.0";
            // 
            // minimumResultLabel
            // 
            this.minimumResultLabel.AutoSize = true;
            this.minimumResultLabel.Location = new System.Drawing.Point(120, 51);
            this.minimumResultLabel.Name = "minimumResultLabel";
            this.minimumResultLabel.Size = new System.Drawing.Size(22, 13);
            this.minimumResultLabel.TabIndex = 12;
            this.minimumResultLabel.Text = "0.0";
            // 
            // maximumResultLabel
            // 
            this.maximumResultLabel.AutoSize = true;
            this.maximumResultLabel.Location = new System.Drawing.Point(120, 27);
            this.maximumResultLabel.Name = "maximumResultLabel";
            this.maximumResultLabel.Size = new System.Drawing.Size(22, 13);
            this.maximumResultLabel.TabIndex = 11;
            this.maximumResultLabel.Text = "0.0";
            // 
            // skewnessLabel
            // 
            this.skewnessLabel.AutoSize = true;
            this.skewnessLabel.Location = new System.Drawing.Point(16, 147);
            this.skewnessLabel.Name = "skewnessLabel";
            this.skewnessLabel.Size = new System.Drawing.Size(59, 13);
            this.skewnessLabel.TabIndex = 10;
            this.skewnessLabel.Text = "Skewness:";
            // 
            // stdDeviationLabel
            // 
            this.stdDeviationLabel.AutoSize = true;
            this.stdDeviationLabel.Location = new System.Drawing.Point(16, 123);
            this.stdDeviationLabel.Name = "stdDeviationLabel";
            this.stdDeviationLabel.Size = new System.Drawing.Size(74, 13);
            this.stdDeviationLabel.TabIndex = 9;
            this.stdDeviationLabel.Text = "Std Deviation:";
            // 
            // varianceLabel
            // 
            this.varianceLabel.AutoSize = true;
            this.varianceLabel.Location = new System.Drawing.Point(16, 99);
            this.varianceLabel.Name = "varianceLabel";
            this.varianceLabel.Size = new System.Drawing.Size(52, 13);
            this.varianceLabel.TabIndex = 8;
            this.varianceLabel.Text = "Variance:";
            // 
            // meanLabel
            // 
            this.meanLabel.AutoSize = true;
            this.meanLabel.Location = new System.Drawing.Point(16, 75);
            this.meanLabel.Name = "meanLabel";
            this.meanLabel.Size = new System.Drawing.Size(37, 13);
            this.meanLabel.TabIndex = 7;
            this.meanLabel.Text = "Mean:";
            // 
            // minimumLabel
            // 
            this.minimumLabel.AutoSize = true;
            this.minimumLabel.Location = new System.Drawing.Point(16, 51);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(51, 13);
            this.minimumLabel.TabIndex = 6;
            this.minimumLabel.Text = "Minimum:";
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.Location = new System.Drawing.Point(16, 27);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(54, 13);
            this.maxLabel.TabIndex = 5;
            this.maxLabel.Text = "Maximum:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.intervalsLabel);
            this.tabPage2.Controls.Add(this.intervalsNumericUpDown);
            this.tabPage2.Controls.Add(this.histogramZedGraphControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(479, 237);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Histogram";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // intervalsLabel
            // 
            this.intervalsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.intervalsLabel.AutoSize = true;
            this.intervalsLabel.Location = new System.Drawing.Point(299, 14);
            this.intervalsLabel.Name = "intervalsLabel";
            this.intervalsLabel.Size = new System.Drawing.Size(98, 13);
            this.intervalsLabel.TabIndex = 19;
            this.intervalsLabel.Text = "Number of intervals";
            // 
            // intervalsNumericUpDown
            // 
            this.intervalsNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.intervalsNumericUpDown.Location = new System.Drawing.Point(410, 12);
            this.intervalsNumericUpDown.Name = "intervalsNumericUpDown";
            this.intervalsNumericUpDown.Size = new System.Drawing.Size(63, 20);
            this.intervalsNumericUpDown.TabIndex = 18;
            // 
            // histogramZedGraphControl
            // 
            this.histogramZedGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.histogramZedGraphControl.Location = new System.Drawing.Point(9, 43);
            this.histogramZedGraphControl.Name = "histogramZedGraphControl";
            this.histogramZedGraphControl.ScrollGrace = 0D;
            this.histogramZedGraphControl.ScrollMaxX = 0D;
            this.histogramZedGraphControl.ScrollMaxY = 0D;
            this.histogramZedGraphControl.ScrollMaxY2 = 0D;
            this.histogramZedGraphControl.ScrollMinX = 0D;
            this.histogramZedGraphControl.ScrollMinY = 0D;
            this.histogramZedGraphControl.ScrollMinY2 = 0D;
            this.histogramZedGraphControl.Size = new System.Drawing.Size(469, 188);
            this.histogramZedGraphControl.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.scatterPlotZedGraphControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(479, 237);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Scatter Plot";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // scatterPlotZedGraphControl
            // 
            this.scatterPlotZedGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scatterPlotZedGraphControl.Location = new System.Drawing.Point(3, 3);
            this.scatterPlotZedGraphControl.Name = "scatterPlotZedGraphControl";
            this.scatterPlotZedGraphControl.ScrollGrace = 0D;
            this.scatterPlotZedGraphControl.ScrollMaxX = 0D;
            this.scatterPlotZedGraphControl.ScrollMaxY = 0D;
            this.scatterPlotZedGraphControl.ScrollMaxY2 = 0D;
            this.scatterPlotZedGraphControl.ScrollMinX = 0D;
            this.scatterPlotZedGraphControl.ScrollMinY = 0D;
            this.scatterPlotZedGraphControl.ScrollMinY2 = 0D;
            this.scatterPlotZedGraphControl.Size = new System.Drawing.Size(473, 234);
            this.scatterPlotZedGraphControl.TabIndex = 13;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.distrubutionComboBox);
            this.tabPage4.Controls.Add(this.ppPlotZedGraphControl);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(479, 237);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "P-P Plot";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Distribution";
            // 
            // distrubutionComboBox
            // 
            this.distrubutionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.distrubutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.distrubutionComboBox.FormattingEnabled = true;
            this.distrubutionComboBox.Location = new System.Drawing.Point(287, 8);
            this.distrubutionComboBox.Name = "distrubutionComboBox";
            this.distrubutionComboBox.Size = new System.Drawing.Size(186, 21);
            this.distrubutionComboBox.TabIndex = 16;
            this.distrubutionComboBox.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // ppPlotZedGraphControl
            // 
            this.ppPlotZedGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ppPlotZedGraphControl.Location = new System.Drawing.Point(9, 43);
            this.ppPlotZedGraphControl.Name = "ppPlotZedGraphControl";
            this.ppPlotZedGraphControl.ScrollGrace = 0D;
            this.ppPlotZedGraphControl.ScrollMaxX = 0D;
            this.ppPlotZedGraphControl.ScrollMaxY = 0D;
            this.ppPlotZedGraphControl.ScrollMaxY2 = 0D;
            this.ppPlotZedGraphControl.ScrollMinX = 0D;
            this.ppPlotZedGraphControl.ScrollMinY = 0D;
            this.ppPlotZedGraphControl.ScrollMinY2 = 0D;
            this.ppPlotZedGraphControl.Size = new System.Drawing.Size(464, 188);
            this.ppPlotZedGraphControl.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "DataSourceCtCtTable";
            // 
            // tableComboBox
            // 
            this.tableComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableComboBox.FormattingEnabled = true;
            this.tableComboBox.Location = new System.Drawing.Point(15, 28);
            this.tableComboBox.Name = "tableComboBox";
            this.tableComboBox.Size = new System.Drawing.Size(186, 21);
            this.tableComboBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Grouping Columns";
            // 
            // groupingColumnsCheckedListBox
            // 
            this.groupingColumnsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupingColumnsCheckedListBox.FormattingEnabled = true;
            this.groupingColumnsCheckedListBox.Items.AddRange(new object[] {
            "asd",
            "as",
            "das",
            "d",
            "asd",
            "asd"});
            this.groupingColumnsCheckedListBox.Location = new System.Drawing.Point(15, 126);
            this.groupingColumnsCheckedListBox.Name = "groupingColumnsCheckedListBox";
            this.groupingColumnsCheckedListBox.Size = new System.Drawing.Size(186, 94);
            this.groupingColumnsCheckedListBox.TabIndex = 5;
            this.groupingColumnsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.GroupingColumnsCheckedListBox_ItemCheck);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Data Column";
            // 
            // dataColumnComboBox
            // 
            this.dataColumnComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataColumnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataColumnComboBox.FormattingEnabled = true;
            this.dataColumnComboBox.Location = new System.Drawing.Point(15, 77);
            this.dataColumnComboBox.Name = "dataColumnComboBox";
            this.dataColumnComboBox.Size = new System.Drawing.Size(186, 21);
            this.dataColumnComboBox.TabIndex = 9;
            this.dataColumnComboBox.SelectedIndexChanged += new System.EventHandler(this.DataColumnComboBox_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.Location = new System.Drawing.Point(15, 274);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(451, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Preview Group";
            // 
            // previewGroupComboBox
            // 
            this.previewGroupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.previewGroupComboBox.Enabled = false;
            this.previewGroupComboBox.FormattingEnabled = true;
            this.previewGroupComboBox.Location = new System.Drawing.Point(534, 281);
            this.previewGroupComboBox.Name = "previewGroupComboBox";
            this.previewGroupComboBox.Size = new System.Drawing.Size(177, 21);
            this.previewGroupComboBox.TabIndex = 16;
            this.previewGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // GroupingAndPreviewEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 311);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.previewGroupComboBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataColumnComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tableComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupingColumnsCheckedListBox);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GroupingAndPreviewEditor";
            this.Text = "Grouping Settings and Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsEditor_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervalsNumericUpDown)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ZedGraphControl histogramZedGraphControl;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label stdDeviationLabel;
        private Label varianceLabel;
        private Label meanLabel;
        private Label minimumLabel;
        private Label maxLabel;
        private Label label2;
        private ComboBox tableComboBox;
        private Label label1;
        private CheckedListBox groupingColumnsCheckedListBox;
        private Label label3;
        private ComboBox dataColumnComboBox;
        private Button okButton;
        private ZedGraphControl scatterPlotZedGraphControl;
        private Label label7;
        private ComboBox distrubutionComboBox;
        private ZedGraphControl ppPlotZedGraphControl;
        private Label label6;
        private ComboBox previewGroupComboBox;
        private NumericUpDown intervalsNumericUpDown;
        private Label intervalsLabel;
        private Label skewnessLabel;
        private Label skewnessResultLabel;
        private Label stdDeviationResultLabel;
        private Label varianceResultLabel;
        private Label meanResultLabel;
        private Label minimumResultLabel;
        private Label maximumResultLabel;
        private Label sampleSizeLabel;
        private Label sampleSizeResultLabel;
        private ListBox rSquaresListBox;
        private Label rSquaresLabel;
    }
}