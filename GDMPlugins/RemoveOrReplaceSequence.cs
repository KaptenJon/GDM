using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using GDMInterfaces;

namespace GDMPlugins
{
    public class RemoveOrReplaceSequence : ITool
    {
        public bool NeedColumnSelected => true;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return t == typeof(string);
        }

        public string ToolCategory => "Remove";

        public string Description => "Removes or replaces a specified character sequence";

        public string Version => "1.0";

        public string Name => "Remove/Replace Sequence";

        public Image Icon => Icons.RemoveOrReplaceSequence;

        public PluginSettings GetSettings(IModel model)
        {
            return new RemoveOrReplaceSequenceSettings();
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
            
            RemoveOrReplaceSequenceSettings settings = (RemoveOrReplaceSequenceSettings)pluginSettings;
            if (model.SelectedTable != null) settings.TableName = model.SelectedTable.TableName;
            if (model.SelectedColumnName != null) settings.ColumnName = model.SelectedColumnName;
        }

        public Type GetSettingsType()
        {
            return typeof(RemoveOrReplaceSequenceSettings);
        }

        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            RemoveOrReplaceSequenceSettings settings = (RemoveOrReplaceSequenceSettings)pluginSettings;
            DataTable table = model.GetTable(settings.TableName);

            switch (settings.RowSelection)
            {
                case RemoveOrReplaceSequenceSettings.Selection.All:
                    int nbrOfRows = table.Rows.Count;
                    status.InitStatus("Removes/Replaces sequence...", nbrOfRows);

                    for (int row = 0; row < nbrOfRows; row++)
                    {
                        WorkHorse(table, settings, row);
                        status.Increment();
                    }
                    break;

                case RemoveOrReplaceSequenceSettings.Selection.Specific:
                    WorkHorse(table, settings, settings.RowNumber - 1);
                    break;
            }
        }

        private void WorkHorse(DataTable table, RemoveOrReplaceSequenceSettings settings, int row)
        {
            string cell = table.Rows[row][settings.ColumnName].ToString();

            switch (settings.RemoveOption)
            {
                case RemoveOrReplaceSequenceSettings.Option.BetweenTags:
                    List<Tuple2> tags = new List<Tuple2>();
                    int start = 0, end = -1;

                    while (true)
                    {
                        start = cell.IndexOf(settings.StartTag, end + 1);
                        end = cell.IndexOf(settings.EndTag, start + 1);

                        if (start != -1 && end != -1) tags.Add(new Tuple2(start, end));
                        else break;
                    }

                    string newSequence = "";    // remove as default
                    if (settings.NewSequence != null) newSequence = settings.NewSequence;

                    foreach (Tuple2 tuple in tags)
                    {
                        string subString = cell.Substring(tuple.Start, tuple.End - tuple.Start);
                        cell = cell.Replace(subString, subString.Replace(settings.Sequence, newSequence));
                    }

                    table.Rows[row][settings.ColumnName] = cell;
                    break;

                case RemoveOrReplaceSequenceSettings.Option.AllOccurences:
                    if (cell.Contains(settings.Sequence))
                    {
                        cell = cell.Replace(settings.Sequence, settings.NewSequence);
                        table.Rows[row][settings.ColumnName] = cell.Replace(settings.Sequence, settings.NewSequence);
                    }
                    break;
            }
        }

        private class Tuple2
        {
            public int Start;
            public int End;

            public Tuple2(int start, int end)
            {
                Start = start; End = end;
            }
        }
    }
}
