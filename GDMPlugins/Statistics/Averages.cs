﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GDMInterfaces;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;

namespace GDMPlugins.Statistics
{
    public class Averages : ITool
    {

        public Averages()
        {

        }


        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Statistics";

        public string Description => "Statistics";

        public string Version => "1.0";

        public string Name => "Averages";

        public Image Icon => Icons.Statistics;

        public PluginSettings GetSettings(IModel model)
        {
            return new StatisticRowOperationSettings(model);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }

        public Type GetSettingsType()
        {
            return typeof(StatisticRowOperationSettings);
        }



        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = (StatisticRowOperationSettings) pluginSettings;
            DataTable table = model.GetTable(settings.DataSourceTable);
            var targettable = model.GetTable(settings.TargetTable);
            if (targettable == null)
            {
                targettable = model.CreateTable();
                targettable.TableName = settings.TargetTable;


            }
            if (!targettable.Columns.Contains("Operation"))
                targettable.Columns.Add("Operation");
            if (!targettable.Columns.Contains(settings.TargetColumn))
            {
                targettable.Columns.Add(settings.TargetColumn,typeof(double));
            }
            List<string> distinct = new List<string>(); // List with distinct values

            var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumn] into g select g;

            foreach (var machine in i)
            {
                var observations =
                    machine.Where(t => IsNumeric(t[settings.DataColumn]))
                        .Select(t => Convert.ToDouble(t[settings.DataColumn]));

                var row = targettable.AsEnumerable().FirstOrDefault(t => t["Operation"] == machine.Key);
                if (row == null)
                {
                    row = targettable.NewRow();
                    targettable.Rows.Add(row);
                }

                row[settings.TargetColumn] = observations.Average();
                row["Operation"] = machine.Key;
                targettable.AcceptChanges();
            }


        }
        public static bool IsNumeric(object value)
        {
            if (!(value is Byte ||
                  value is Int16 ||
                  value is Int32 ||
                  value is Int64 ||
                  value is SByte ||
                  value is UInt16 ||
                  value is UInt32 ||
                  value is UInt64 ||
                  value is BigInteger ||
                  value is Decimal ||
                  value is Double ||
                  value is Single))
                return false;
            else
                return true;
        }
    }

    public class AveragesConfidence : ITool
    {

        public AveragesConfidence()
        {

        }


        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }

        public string ToolCategory => "Statistics";

        public string Description => "Statistics";

        public string Version => "1.0";

        public string Name => "Averages With Confidence Intervall";

        public Image Icon => Icons.Statistics;

        public PluginSettings GetSettings(IModel model)
        {
            return new StatisticRowOperationSettings(model);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }

        public Type GetSettingsType()
        {
            return typeof(StatisticRowOperationSettings);
        }



        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            var settings = (StatisticRowOperationSettings) pluginSettings;
            DataTable table = model.GetTable(settings.DataSourceTable);
            var targettable = model.GetTable(settings.TargetTable);
            if (targettable == null)
            {
                targettable = model.CreateTable();
                targettable.TableName = settings.TargetTable;


            }
            if (!targettable.Columns.Contains("Operation"))
                targettable.Columns.Add("Operation");

            if (!targettable.Columns.Contains(settings.TargetColumn ))
            {
                targettable.Columns.Add(settings.TargetColumn , typeof(double));
            }
            if (!targettable.Columns.Contains(settings.TargetColumn + " Confidece Intervall"))
            {
                targettable.Columns.Add(settings.TargetColumn + " Confidece Intervall", typeof(double));
            }
            List<string> distinct = new List<string>(); // List with distinct values

            var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumn] into g select g;

            foreach (var machine in i)
            {
                var observations =
                    machine.Where(t => IsNumeric(t[settings.DataColumn]))
                        .Select(t => Convert.ToDouble(t[settings.DataColumn]));

                var row =
                    targettable.AsEnumerable()
                        .FirstOrDefault(t => Equals(t["Operation"].ToString(), machine.Key.ToString()));
                if (row == null)
                {
                    row = targettable.NewRow();
                    targettable.Rows.Add(row);
                }

                row[settings.TargetColumn] = observations.Average();

                double T = StudentT.InvCDF(0, 1, observations.Count(), 0.9);
                double sd = observations.StandardDeviation();
                double c = T*(sd/Math.Sqrt(observations.Count()));
                row[settings.TargetColumn + " Confidece Intervall"] = c;
                targettable.AcceptChanges();
            }


        }


        public static bool IsNumeric(object value)
        {
            if (!(value is Byte ||
                  value is Int16 ||
                  value is Int32 ||
                  value is Int64 ||
                  value is SByte ||
                  value is UInt16 ||
                  value is UInt32 ||
                  value is UInt64 ||
                  value is BigInteger ||
                  value is Decimal ||
                  value is Double ||
                  value is Single))
                return false;
            else
                return true;
        }


    }

}
