using System;
using System.Windows.Forms;

namespace GDMPlugins
{
    public partial class DateIntervalEditor : Form
    {
        private DateIntervalSettings _settings;
        private const int LowerBound = 0;
        private const int Interval = 1;
        private const int UpperBound = 2;

        public DateIntervalEditor(DateIntervalSettings settings)
        {
            InitializeComponent();
            _settings = settings;
        }

        private void DateIntervalEditor_Load(object sender, EventArgs e)
        {
            filterModeComboBox.Items.Add("Lower Bound");
            filterModeComboBox.Items.Add("Interval");
            filterModeComboBox.Items.Add("Upper Bound");
            filterModeComboBox.SelectedIndex = Interval;

            for (int i = 1; i < 24; i++)
            {
                string hour;
                if (i < 10)
                    hour = "0" + i;
                else hour = "" + i;
                hourComboBox.Items.Add(hour);
            }
            hourComboBox.Items.Add("00");
            hourComboBox.SelectedIndex = 0;
            monthCalendar.Visible = false;

            for (int i = 0; i < 60; i++)
            {
                string min;
                if (i < 10)
                    min = "0" + i;
                else min = "" + i;
                minuteComboBox.Items.Add(min);
            }
            minuteComboBox.SelectedIndex = 0;

            for (int i = 0; i < 60; i++)
            {
                string sec;
                if (i < 10)
                    sec = "0" + i;
                else sec = "" + i;
                secondsComboBox.Items.Add(sec);
            }
            secondsComboBox.SelectedIndex = 0;
            

            for (int i = 0; i < 365; i++)
            {
                string sec;
                if (i < 10)
                    sec = "0" + i;
                else sec = "" + i;
                days.Items.Add(sec);
            }
            days.SelectedIndex = 0;
            years.SelectedIndex = 0;

            lowerBoundTextBox.ReadOnly = true;
            upperBoundTextBox.ReadOnly = true;
            RelativeAbsoluteCombo.SelectedIndex = 0;
            filterModeComboBox.SelectedIndexChanged += filterModeComboBox_SelectedIndexChanged;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(lowerBoundRadioButton.Checked)
                lowerBoundTextBox.Text = "";
            else if(upperBoundRadioButton.Checked)
                upperBoundTextBox.Text = "";
        }

        private void filterModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (filterModeComboBox.SelectedIndex)
            {
                case LowerBound:
                    lowerBoundRadioButton.Checked = true;
                    lowerBoundRadioButton.Visible = true;
                    upperBoundRadioButton.Checked = false;
                    upperBoundRadioButton.Visible = false;
                    lowerBoundLabel.Text = "Lower Bound";
                    upperBoundLabel.Text = "";
                    upperBoundTextBox.Text = "";
                    break;
                case Interval:
                    lowerBoundRadioButton.Checked = true;
                    lowerBoundRadioButton.Visible = true;
                    upperBoundRadioButton.Checked = false;
                    upperBoundRadioButton.Visible = true;
                    lowerBoundLabel.Text = "From";
                    upperBoundLabel.Text = "To";
                    upperBoundTextBox.Text = "";
                    lowerBoundTextBox.Text = "";
                    break;
                case UpperBound:
                    lowerBoundRadioButton.Checked = false;
                    lowerBoundRadioButton.Visible = false;
                    upperBoundRadioButton.Checked = true;
                    upperBoundRadioButton.Visible = true;
                    lowerBoundLabel.Text = "";
                    upperBoundLabel.Text = "Upper Bound";
                    lowerBoundTextBox.Text = "";
                    break;
            }
        }

        private DateTime ConvertToDateTime(string date)
        { 
      DateTime datetime = DateTime.MinValue;
            try
            {
                datetime = DateTime.ParseExact(date, "yyyy:MM:dd HH:mm:ss.fff", null);
            }
            catch
            {
                // Not nullable so MinValue is used to signal error
                return DateTime.MinValue;
            }

            return datetime;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            switch (filterModeComboBox.SelectedIndex)
            {
                case LowerBound:
                    _settings.Minimum = ConvertToDateTime(lowerBoundTextBox.Text).ToString("s");
                    _settings.Mode = DateIntervalSettings.DateFilterMode.LowerBound;
                    break;
                case Interval:
                    _settings.Minimum = ConvertToDateTime(lowerBoundTextBox.Text).ToString("s");
                    _settings.Maximum = ConvertToDateTime(upperBoundTextBox.Text).ToString("s");
                    _settings.Mode = DateIntervalSettings.DateFilterMode.Interval;
                    break;
                case UpperBound:
                    _settings.Maximum = ConvertToDateTime(upperBoundTextBox.Text).ToString("s");
                    _settings.Mode = DateIntervalSettings.DateFilterMode.UpperBound;
                    break;
            }
            
            Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            int ms;
            try
            {
                ms = Convert.ToInt32(milliSecondsTextBox.Text);
            }
            catch
            {
                MessageBox.Show("The MilliSeconds field is not valid.","Errorous input",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (ms < 0 || ms > 999)
            {
                MessageBox.Show("The MilliSeconds field is not valid.", "Errorous input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string dateTime;
            if (RelativeAbsoluteCombo.SelectedIndex == 1)
                dateTime = monthCalendar.SelectionStart.ToString("yyyy:MM:dd") + " " +
                           hourComboBox.SelectedItem + ":" + minuteComboBox.SelectedItem + ":" +
                           secondsComboBox.SelectedItem + "." +
                           milliSecondsTextBox.Text;
            else
            {
                var t = TimeSpan.FromDays(int.Parse(years.SelectedItem.ToString())*365 + int.Parse(days.SelectedItem.ToString())) +
                        TimeSpan.FromHours(int.Parse(hourComboBox.SelectedItem.ToString())) +
                        TimeSpan.FromMinutes(int.Parse(minuteComboBox.SelectedItem.ToString())) +
                        TimeSpan.FromSeconds(int.Parse(minuteComboBox.SelectedItem.ToString())) + TimeSpan.FromMilliseconds(ms);

                dateTime = "[CurrentDateTime-" + t.ToString(@"dddd\ hh\:mm\:ss\.fff") + "]";
            }
                

            if (lowerBoundRadioButton.Checked)
            {
                lowerBoundTextBox.Text = dateTime;
                upperBoundRadioButton.Checked = true;
                lowerBoundRadioButton.Checked = false;
            }
            else if (upperBoundRadioButton.Checked)
            {
                upperBoundTextBox.Text = dateTime;
                lowerBoundRadioButton.Checked = true;
                upperBoundRadioButton.Checked = false;
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            monthCalendar.Visible = RelativeAbsoluteCombo.SelectedIndex == 1;
        }
    }
}
