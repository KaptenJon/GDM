using System;
using System.Drawing;
using System.Windows.Forms;
using GDMCore;

namespace GDMTool
{
    /// <summary>
    /// Displays the log and let the user print and filter the log file.
    /// </summary>
    public partial class LogViewer : Form
    {
        private Controller _controller;

        public LogViewer(Controller controller)
        {
            InitializeComponent();
            _controller = controller;

            chapterComboBox.Items.Add("All"); int i = 1;
            foreach (Log.LogChapter chapter in _controller.Log.LogChapters)
            {
                chapterComboBox.Items.Add(i + ". " + chapter.Plugin);
                i++;
            }
            chapterComboBox.SelectedIndex = 0;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChapterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chapterComboBox.SelectedIndex == 0)
            {
                string str = "";

                foreach (Log.LogChapter chapter in _controller.Log.LogChapters)
                {
                    str += FormatChapter(chapter);
                }

                logRichTextBox.Text = str;
            }
            else
            {
                int index = chapterComboBox.SelectedIndex - 1;
                logRichTextBox.Text = FormatChapter(_controller.Log.LogChapters[index]);
            }
        }

        private string FormatChapter(Log.LogChapter chapter)
        {
            if (chapter == null) return "";

            string str = chapter.Plugin + "\n";

            foreach (Log.LogEntry entry in chapter.LogEntries)
            {
                str += entry.Type + ": " + entry.Message + "\n";
            }

            str += "\n";

            return str;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "log files (*.log)|*.log"; 
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = "log";

            if (dialog.ShowDialog() == DialogResult.OK)
                _controller.WriteLog(dialog.FileName);
        }

        private void LogViewer_Resize(object sender, EventArgs e)
        {
            logRichTextBox.Size = new Size(Width - logRichTextBox.Location.X - 28, Height - logRichTextBox.Location.Y - 48);
        }
    }
}
