using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Statistics.Analysis;
using Accord.Statistics.Distributions;
using GDMInterfaces;
using GoodnessOfFit = MathNet.Numerics.GoodnessOfFit;

namespace GDMPlugins.Statistics
{
	public class GenerateDistributions : ITool
	{


		public GenerateDistributions()
		{

		}



		public bool NeedColumnSelected => false;

		public bool NeedTableSelected => true;

		public bool AcceptsDataType(Type t)
		{
			return true;
		}

		public string ToolCategory => "Statistics";

		public string Description => "Generates Statistical Distributions";

		public string Version => "1.0";

		public string Name => "Generate Distribution";

		public Image Icon => Icons.Statistics;

		public PluginSettings GetSettings(IModel model)
		{
			return new GenerateDistributionSettings(model);
		}

		public void UpdateSettings(PluginSettings pluginSettings, IModel model)
		{
		}

		public Type GetSettingsType()
		{
			return typeof (GenerateDistributionSettings);
		}

		/// <summary>
		/// Given grouping columns with corresponding values, the method filters out uninterested data 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="dataColumn">Data column</param>
		/// <param name="columnValues">The data values we're interested in</param>
		/// <param name="columnNames">The grouping columns' names</param>
		/// <returns>List with relevant data values</returns>
		internal static List<double> ExtractData(DataTable table, string dataColumn, string[] columnValues,
			string[] columnNames)
		{
			List<double> observations = new List<double>();

			foreach (DataRow row in table.Rows)
			{
				if ((columnValues == null || columnValues.Length == 0) && row[dataColumn] != DBNull.Value) // All rows
					observations.Add(Convert.ToDouble(row[dataColumn]));
				else // Filter out relevant rows
				{
					for (int i = 0; i < columnNames.Length; i++)
					{
						if (row[columnNames[i]].ToString() == columnValues[i])
						{
							if (row[columnNames[i]] != DBNull.Value)
								observations.Add(Convert.ToDouble(row[dataColumn]));
						}
					}
				}
			}

			return observations;
		}

		

		private static string GroupingString(string[] columnValues)
		{
			string str = "";
			bool firstRun = true;

			foreach (string value in columnValues)
			{
				if (firstRun)
				{
					str += value;
					firstRun = false;
				}
				else str += ", " + value;
			}

			return "{" + str + "}";
		}


	    public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
	    {
	        GenerateDistributionSettings settings = (GenerateDistributionSettings) pluginSettings;

	        string[] distributions = null;
#pragma warning disable 618
	        if (settings.Distributions != null && settings.Distributions.Length > 0)

	            distributions = settings.Distributions;
	        else
	        {
	            if (settings.Distribution == "Best Fit")
	                distributions = DistributionEvaluator.GetAllDistributions().Select(t => t.GetType().Name).ToArray();
	            else
	                distributions = new[] {settings.Distribution};
	        }

#pragma warning restore 618

	        status.InitStatus("Generate Distribution", distributions.Length);

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
	            targettable.Columns.Add(settings.TargetColumn);
	        }
	        List<string> distinct = new List<string>(); // List with distinct values
	        if (table == null)
	        {
	            log.Add(LogType.Error,
	                "Facts operation did not work, the table " + settings.DataSourceTable + " did not exsist");
                return;
	        }

	    

	        var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumn] into g select g;

		    var machines = i as IList<IGrouping<object, DataRow>> ?? i.ToList();
		    status.InitStatus("Generate Distribution", machines.Count());

            var cult = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
			var precult = Thread.CurrentThread.CurrentCulture;
			if (cult != null)
			{
				cult.NumberFormat.CurrencyDecimalSeparator = settings.Decimal;
				Thread.CurrentThread.CurrentCulture = cult;
			}

			foreach (var machine in machines)
			{
                status.Increment();
				DataRow row = null;
				try
				{
					if (distinct.Contains(machine.Key.ToString())) continue; // Already calculated -> next row
					else distinct.Add(machine.Key.ToString());
					var observations =
						machine.Where(t => IsNumeric(t[settings.DataColumn]))
							.Select(t => Convert.ToDouble(t[settings.DataColumn]));
					if (settings.Filter != null)
						observations =
							machine.Where(
								t =>
									IsNumeric(t[settings.DataColumn]) &&
									t[settings.FilterColumn].ToString().Trim() == settings?.Filter)
								.Select(t => Convert.ToDouble(t[settings.DataColumn]));

					DistributionData distributionData = DistributionEvaluator.BestDistribution(observations, distributions, log);


					row = targettable.AsEnumerable()
						.FirstOrDefault(t => t["Operation"].ToString().Trim() == machine.Key.ToString().Trim());
					if (row == null)
					{
						row = targettable.NewRow();
						targettable.Rows.Add(row);
					}
					if (distributionData.Distribution == null)
						row[settings.TargetColumn] = "No Data";
					else
						row[settings.TargetColumn] = distributionData.Distribution.ToString() + " " + settings.Unit;
					row["Operation"] = machine.Key.ToString().Trim();
					targettable.AcceptChanges();
				}
				catch
				{
					if (row != null) row[settings.TargetColumn] = "No Data";
				}
			}
			Thread.CurrentThread.CurrentCulture = precult;

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
