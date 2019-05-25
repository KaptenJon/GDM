using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// Implements the ILog interface. Contains the data structures that store the log entries and methods to write the log to a file.
    /// </summary>
    public class Log : ILog
    {
        private List<LogChapter> _logChapters = new List<LogChapter>();
        private LogChapter _currentChapter = null;
        private Controller _controller;
        public static Boolean EventLog;
        public LogChapter CurrentChapter => _currentChapter;

        public List<LogChapter> LogChapters => _logChapters;

        public Log(Controller controller)
        {
            _controller = controller;
        }

        public void Clear()
        {
            _logChapters.Clear();
        }

        public void CreateChapter(string plugin)
        {
            if (EventLog)
            {
                var eventType =  EventLogEntryType.Information;
                System.Diagnostics.EventLog.WriteEntry("GDM Service", plugin, eventType);
                return;
            }
            LogChapter chapter = new LogChapter();
            chapter.Plugin = plugin;
            chapter.LogEntries = new List<LogEntry>();
            _logChapters.Add(chapter);
            _currentChapter = chapter;
        }

        public void Add(LogType type, string message)
        {
            if (EventLog)
            {
                var eventType = type == LogType.Error
                    ? EventLogEntryType.Error
                    : type == LogType.Warning ? EventLogEntryType.Warning : EventLogEntryType.Information;
                System.Diagnostics.EventLog.WriteEntry("GDM Service",message, eventType);
                return;
            }
            if (_currentChapter == null) return;
            LogEntry entry = new LogEntry();
            entry.Message = message;
            entry.Type = type;
            lock(_currentChapter.LogEntries)
                _currentChapter.LogEntries.Add(entry);
            if (type == LogType.Error && PluginSettings.IsInUIMode)
                MessageBox.Show(entry.Message, "Error");
        }

        public void Add(LogType type, Exception e)
        {
            string message = e.Message + "\n\n" + e.StackTrace;
            Add(type, message);
        }

        public void Flush(string filename)
        {
            using (StreamWriter f = File.CreateText(filename))
            {
                foreach (LogChapter chapter in _logChapters)
                {
                    f.WriteLine(chapter.Plugin);
                    for (int i = 0; i < chapter.Plugin.Length; i++) f.Write('-');
                    f.WriteLine();
                    foreach (LogEntry entry in chapter.LogEntries)
                    {
                        f.WriteLine(entry.Type.ToString() + "," + entry.Message);
                        
                    }
                    f.WriteLine();
                }
            }
        }

        public class LogEntry
        {
            public string Message;
            public LogType Type;
        }

        public class LogChapter
        {
            public string Plugin;
            public List<LogEntry> LogEntries;
        }
    }
}
