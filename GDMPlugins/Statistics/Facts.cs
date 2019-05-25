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
    public class Facts : IOutput
    {
        public static List<IUnivariateDistribution> _distributions;
      
        public Facts()
        {
            // Add new distributions here...
            _distributions = new List<IUnivariateDistribution>(new IUnivariateDistribution[]
            {
                new WeibullDistribution(1,1),
                new ExponentialDistribution(),
                new GammaDistribution(),
                new TriangularDistribution(0,1,0.5),
                new UniformContinuousDistribution(),
                new LognormalDistribution(),
                new NormalDistribution(),
                new BetaDistribution(),
                new ConstantDistribution(), 
            });
        }


        public bool NeedColumnSelected => false;

        public bool NeedTableSelected => true;

        public bool AcceptsDataType(Type t)
        {
            return true;
        }


        public string Description => "Statistical analysis";

        public string Version => "1.0";

        public string Name => "Facts Exporter";

        public Image Icon => Icons.Statistics;

        public PluginSettings GetSettings(IModel model)
        {
            return new FactsSettings(model);
        }

        public void UpdateSettings(PluginSettings pluginSettings, IModel model)
        {
        }

        public Type GetSettingsType()
        {
            return typeof (FactsSettings);
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

        ///// <summary>
        ///// Determines the best suitable distribution
        ///// </summary>
        ///// <param name="ob">List with observed vales</param>
        ///// <param name="settings"></param>
        ///// <param name="log"></param>
        ///// <returns>Object with location, scale, shape and RSquare value</returns>
        //private DistributionData BestDistribution(IEnumerable<double> ob, string distributionParam,
        //     ILog log)
        //{
        //    var observations = ob.ToList();
        //    IEnumerable<DistributionData> distributionsData = EvaluateDistributions(observations, log, distributionParam);
        //    if (distributionsData == null) return null;
        //    DistributionData best = null;

        //    foreach (DistributionData distribution in distributionsData)
        //    {
        //        // the one with largest RSquare will be selected
        //        if (best == null || distribution.RSquare > best.RSquare)
        //            best = distribution;
        //    }


        //    if (log != null && observations.Count <= 10)
        //        log.Add(LogType.Warning,
        //            "The result is insecure due to a low number of observations");

        //    return best;
        //}

        ///// <summary>
        ///// Calculates location, scale, shape and p value for every present distribution
        ///// </summary>
        ///// <param name="observations">List with observed values</param>
        ///// /// <param name="log">ILog object</param>
        ///// <param name="distributionParam"></param>
        ///// <returns>List with results from MLE and Kolmogorov-Smirnor algorithm</returns>
        //static IEnumerable<DistributionData> EvaluateDistributions(List<double> observations,
        //    ILog log, string distributionParam)
        //{
        //    ConcurrentBag<DistributionData> distributionsData = new ConcurrentBag<DistributionData>();
        //    var analysis = new DistributionAnalysis(observations.ToArray());
        //   // analysis.Compute();
                
           
            
        //    if (distributionParam == null || distributionParam == "Best Fit")
        //    {
        //        Parallel.ForEach(GetDistributions(), (dist) =>
        //        {
        //            var distribution = dist;
        //            try
        //            {
        //                distribution = UnivariateDistribution(observations, distribution);

        //                var R = RSquare(observations, distribution);

        //                //P = new Accord.GenerateDistributions.Testing.KolmogorovSmirnovTest(observations.ToArray(),distribution, KolmogorovSmirnovTestHypothesis.SampleIsDifferent).PValue;
        //                //var e =Evaluate(observations,distribution);
        //                // P = Pvalue(observations.Count, e);
        //                distributionsData.Add(new DistributionData(distribution, R));

        //            }
        //            catch (Exception)
        //            {
        //                if (log != null && observations.Count <= 1)
        //                    log.Add(LogType.Warning,
        //                        "The distribution   could not be determined due to an insufficient number of observations");

        //            }
        //        });
        //    }
        //    else
        //    {
        //       IUnivariateDistribution dist = GetDistributions().Find(t=>t.GetType().Name== distributionParam);
        //        dist = UnivariateDistribution(observations, dist);
                
        //        //var R = Pvalue(observations.Count, Evaluate(observations,dist));
        //       distributionsData.Add(new DistributionData(dist, RSquare(observations, dist)));
        //    }

        //    return distributionsData;
        //}

        //private static double RSquare(List<double> observations, IUnivariateDistribution distribution)
        //{
        //    Random rand = new Random();
        //    var obs = observations.Distinct().ToArray();
        //    var newobs = new double[obs.Count()];
        //    double rd = 0d;
        

        //    try
        //    {
        //        if ((distribution is Accord.Statistics.Distributions.Univariate.BetaDistribution &&
        //            (distribution as Accord.Statistics.Distributions.Univariate.BetaDistribution).Alpha < 0.1) ||
        //            Double.IsNaN(distribution.Mean)|| Double.IsNaN(distribution.Variance) || Double.IsNaN(distribution.Mode))
        //            return 0;
        //        for (int i = 0; i < obs.Count(); i++)
        //        {
                    
        //            rd = rand.NextDouble();
        //            newobs[i] = distribution.InverseDistributionFunction(rd);
        //        }
        //    }
        
        //    catch (ArithmeticException)
        //    {
        //        return 0;}
        //    catch (Exception)
        //    {

        //        return 0;
        //    }
        //    obs.StableSort();
        //    newobs.StableSort();
        //    var r = GoodnessOfFit.RSquared(newobs, obs);
        //    if (Double.IsNaN(r))
        //        r = 0;
        //    return r;
        //}

        //private static IUnivariateDistribution UnivariateDistribution(List<double> observations, IUnivariateDistribution distribution)
        //{
        //    if (distribution is WeibullDistribution)
        //        distribution = MaximumLikelihoodEstimation.Weibull(observations.ToArray());
        //    else if (distribution is TriangularDistribution)
        //        distribution = MaximumLikelihoodEstimation.Triangular(observations);
        //    else if (distribution is BetaDistribution)
        //        distribution = MaximumLikelihoodEstimation.Beta(observations);
        //    else if (distribution is EmpiricalDistribution)
        //    {
        //        distribution = MaximumLikelihoodEstimation.Empirical(observations);
        //    }
        //    else if (distribution is GammaDistribution)
        //    {
        //        distribution = MaximumLikelihoodEstimation.Gamma(observations.ToArray());
        //    }
        //    else
        //        distribution?.Fit(observations.ToArray());
        //    return distribution;
        //}
        

        //public static double Evaluate(List<double> observations, IDistribution dist)
        //{
        //    if (dist == null)
        //        throw new Exception("Distribution not initialized");

        //    double dMaxMinus;
        //    var dMaxPlus = dMaxMinus = Double.MinValue;
        //    List<double> copy = observations.ToList();
        //    int n = copy.Count, j = 1; copy.Sort();

        //    foreach (double x in copy)
        //    {
        //        double t1 = (double)j / n - dist.DistributionFunction(x);
        //        double t2 = dist.DistributionFunction(x) - ((double)(j - 1) / n);
        //        if (t1 > dMaxPlus) dMaxPlus = t1;
        //        if (t2 > dMaxMinus) dMaxMinus = t2;
        //        j++;
        //    }

        //    return Math.Max(dMaxPlus, dMaxMinus);
        //}

        //// Reference: "Robustness and Distribution Assumptions", mathematical statistics department of CTH/GU, (?)
        //public static double Pvalue(int n, double d)
        //{
        //    double x = Math.Sqrt(n) * d, sum = 0, previousSum = Double.MinValue;
        //    int i = 1;

        //    while (Math.Abs(sum - previousSum) >= 0.000001d)
        //    {
        //        previousSum = sum;
        //        sum += Math.Pow(-1, i - 1) * Math.Exp(-2 * i * i * x * x);
        //        i++;
        //    }

        //    return 2 * sum;
        //}

        //private static string GroupingString(string[] columnValues)
        //{
        //    string str = "";
        //    bool firstRun = true;

        //    foreach (string value in columnValues)
        //    {
        //        if (firstRun)
        //        {
        //            str += value;
        //            firstRun = false;
        //        }
        //        else str += ", " + value;
        //    }

        //    return "{" + str + "}";
        //}


        public void Apply(IModel model, PluginSettings pluginSettings, ILog log, IStatus status)
        {
            FactsSettings settings = (FactsSettings) pluginSettings;
            
            var targettable = model.GetTable(settings.TargetTable);
            if (targettable == null)
            {
                targettable = model.CreateTable();
                targettable.TableName = settings.TargetTable;
            }
            if (!targettable.Columns.Contains("Operation"))
                targettable.Columns.Add("Operation");

            var cult = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            var precult = Thread.CurrentThread.CurrentCulture;
            if (cult != null)
            {
                cult.NumberFormat.NumberDecimalSeparator = settings.Decimal;
                Thread.CurrentThread.CurrentCulture = cult;
            }
            
            //CT
            if (!String.IsNullOrEmpty(settings.CTColumn) && !String.IsNullOrEmpty(settings.DataSourceCTTable))
            {
                
                
               if (!targettable.Columns.Contains("CT"))
                {
                    targettable.Columns.Add("CT");
                }
                List<string> distinct = new List<string>(); // List with distinct values

                var table = model.GetTable(settings.DataSourceCTTable);
                if (table == null)
                    log.Add(LogType.Error, "Facts operation did not work, the table " + table.TableName + " did not exsist");

                var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumnCT] into g select g;

                string[] distributions = null;
                if (settings.DistributionsCT != null && settings.DistributionsCT.Length > 0) 
                    distributions = settings.DistributionsCT;
                else
                {
#pragma warning disable 612
                    if (settings.DistributionCT == "Best Fit")

                        distributions = DistributionEvaluator.GetAllDistributions().Select(t => t.GetType().Name).ToArray();
                    else
                        distributions = new[] { settings.DistributionCT };
                }
#pragma warning restore 612
                var enumerable = i as IList<IGrouping<object, DataRow>> ?? i.ToList();
                status.InitStatus("Facts CT", enumerable.Count());
                foreach (var machine in enumerable)
                {
                    status.Increment();
                    if (distinct.Contains(machine.Key.ToString())) continue; // Already calculated -> next row
                    else distinct.Add(machine.Key.ToString());
                    var observations =
                        machine.Where(t => IsNumeric(t[settings.CTColumn]))
                            .Select(t => Convert.ToDouble(t[settings.CTColumn]));




                    DistributionData distributionData = DistributionEvaluator.BestDistribution(observations,  distributions, log);
                    
                    var row =
                        targettable.AsEnumerable()
                            .FirstOrDefault(t => t["Operation"].ToString().Trim() == machine.Key.ToString().Trim());
                    if (row == null)
                    {
                        row = targettable.NewRow();
                        targettable.Rows.Add(row);
                    }
                    if (distributionData.Distribution == null)
                        row["CT"] = "No Data";
                    else
                        row["CT"] = distributionData.Distribution.ToString() + " " + settings.UnitCT;
                    row["Operation"] = machine.Key.ToString().Trim();
                    targettable.AcceptChanges();
                }
            }

            //MTBF
            if (!String.IsNullOrEmpty(settings.MTBFColumn) && !String.IsNullOrEmpty(settings.DataSourceMTBFTable))
            {
               
                
                if (!targettable.Columns.Contains("MTBF"))
                {
                    targettable.Columns.Add("MTBF");
                }
                List<string> distinct = new List<string>(); // List with distinct values
                var table = model.GetTable(settings.DataSourceMTBFTable);
                if (table == null)
                    log.Add(LogType.Error, "Facts operation did not work, the table " + table.TableName + " did not exsist");

                var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumnMTBF] into g select g;

                
#pragma warning disable 612
                string[] distributions = null;

                if (settings.DistributionsMTBF != null && settings.DistributionsMTBF.Length > 0)

                    distributions = settings.DistributionsMTBF;
                else
                {
                    if (settings.DistributionMTBF == "Best Fit")
                        distributions = DistributionEvaluator.GetAllDistributions().Select(t => t.GetType().Name).ToArray();
                    else
                        distributions = new[] { settings.DistributionMTBF };
                }
#pragma warning restore 612
                var machines = i as IList<IGrouping<object, DataRow>> ?? i.ToList();
                status.InitStatus("Facts MTBF", machines.Count());
                foreach (var machine in machines)
                {
                    status.Increment();
                    if (distinct.Contains(machine.Key.ToString())) continue; // Already calculated -> next row
                    else distinct.Add(machine.Key.ToString());
                    var observations =
                        machine.Where(t => IsNumeric(t[settings.MTBFColumn]))
                            .Select(t => Convert.ToDouble(t[settings.MTBFColumn]));

                    DistributionData distributionData = DistributionEvaluator.BestDistribution(observations, distributions,log);

                    var row =
                        targettable.AsEnumerable()
                            .FirstOrDefault(t => t["Operation"].ToString().Trim() == machine.Key.ToString().Trim());
                    if (row == null)
                    {
                        row = targettable.NewRow();
                        targettable.Rows.Add(row);
                    }
                    if (distributionData.Distribution == null)
                        row["MTBF"] = "No Data";
                    else
                        row["MTBF"] = distributionData.Distribution.ToString() + " " + settings.UnitMTBF;
                    row["Operation"] = machine.Key.ToString().Trim();
                    targettable.AcceptChanges();
                }
            }

            //MTTR
            if (!String.IsNullOrEmpty(settings.MTTRColumn) && !String.IsNullOrEmpty(settings.DataSourceMTTRTable))
            {
                
               if (!targettable.Columns.Contains("MTTR"))
                {
                    targettable.Columns.Add("MTTR");
                }
                List<string> distinct = new List<string>(); // List with distinct values
                var table = model.GetTable(settings.DataSourceMTTRTable);
                if (table == null)
                    log.Add(LogType.Error, "Facts operation did not work, the table " + table.TableName + " did not exsist");


#pragma warning disable 612
                string[] distributions = null;

                if (settings.DistributionsMTTR != null && settings.DistributionsMTTR.Length > 0)

                    distributions = settings.DistributionsMTTR;
                else
                {
                    if (settings.DistributionMTTR == "Best Fit")
                        distributions = DistributionEvaluator.GetAllDistributions().Select(t => t.GetType().Name).ToArray();
                    else
                        distributions = new[] { settings.DistributionMTTR };
                }
#pragma warning restore 612

                var i = from p in table.AsEnumerable() group p by p[settings.OperationNameColumnMTTR] into g select g;


                var machines = i as IList<IGrouping<object, DataRow>> ?? i.ToList();
                status.InitStatus("Facts CT", machines.Count());

                foreach (var machine in machines)
                {
                    status.Increment();
                    if (distinct.Contains(machine.Key.ToString())) continue; // Already calculated -> next row
                    else distinct.Add(machine.Key.ToString());
                    var observations =
                        machine.Where(t => IsNumeric(t[settings.MTTRColumn]))
                            .Select(t => Convert.ToDouble(t[settings.MTTRColumn]));

                    var distributionData = DistributionEvaluator.BestDistribution(observations,distributions, log);
                   
                    var row =
                        targettable.AsEnumerable()
                            .FirstOrDefault(t => t["Operation"].ToString().Trim() == machine.Key.ToString().Trim());
                    if (row == null)
                    {
                        row = targettable.NewRow();
                        targettable.Rows.Add(row);
                    }
                    if (distributionData.Distribution == null)
                        row["MTTR"] = "No Data";
                    else
                        row["MTTR"] = distributionData.Distribution.ToString() + " " + settings.UnitMTTR;
                    row["Operation"] = machine.Key.ToString().Trim();
                    targettable.AcceptChanges();
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


        public string GetJobDescription(PluginSettings s)
        {
            return Description;
        }

        public object GetDynamicSettings(PluginSettings s)
        {
            return null;
        }

        public Tag Tags => null;
    }
}
