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
    public class DistributionEvaluator

    {
        private static readonly List<IUnivariateDistribution> _distributions = new List<IUnivariateDistribution>(new IUnivariateDistribution[]
            {
                new WeibullDistribution(1, 1),
                new ExponentialDistribution(),
                new GammaDistribution(),
                new TriangularDistribution(0, 0, 0),
                new UniformContinuousDistribution(),
                new EmpiricalDistribution(new[] {0d, 0d}),
                new LognormalDistribution(),
                new NormalDistribution(),
                new BetaDistribution(),
                new ConstantDistribution(),
            });



        public DistributionEvaluator()
        {
        }

        public static List<IUnivariateDistribution> GetDistributions(string[] distributions)
        {
            return _distributions.Where(t => distributions.Contains(t.GetType().Name)).ToList();
        }

        public static List<IUnivariateDistribution>GetAllDistributions()
        {
            return _distributions;
        }

        public static List<string> GetAllDistributionNames()
        {
             return _distributions.Select(t => t.GetType().Name).ToList(); 
            
        }
        
        /// <summary>
        /// Determines the best suitable distribution
        /// </summary>
        /// <param name="ob">List with observed vales</param>
        /// <param name="settings"></param>
        /// <param name="log"></param>
        /// <returns>Object with location, scale, shape and RSquare value</returns>
        public static DistributionData BestDistribution(IEnumerable<double> ob, string[] distributions,
            ILog log)
        {
            var observations = ob.ToList();
            IEnumerable<DistributionData> distributionsData = EvaluateDistributions(observations, log, distributions);
            if (distributionsData == null) return null;
            DistributionData best = null;

            foreach (DistributionData distribution in distributionsData)
            {
                // the one with largest RSquare will be selected
                if (best == null || distribution.RSquare > best.RSquare)
                    best = distribution;
            }


            if (log != null && observations.Count <= 10)
                log.Add(LogType.Warning,
                    "The result is insecure due to a low number of observations");

            return best;
        }

        /// <summary>
        /// Calculates location, scale, shape and p value for every present distribution
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// /// <param name="log">ILog object</param>
        /// <param name="settings"></param>
        /// <returns>List with results from MLE and Kolmogorov-Smirnor algorithm</returns>
        public static IEnumerable<DistributionData> EvaluateDistributions(List<double> observations,
            ILog log, string[] distributions)
        {
            ConcurrentBag<DistributionData> distributionsData = new ConcurrentBag<DistributionData>();
            //var analysis = new DistributionAnalysis(observations.ToArray());
            // analysis.Compute();

            List<IUnivariateDistribution> distributionsToTest = null;
            if (distributions == null || distributions.Length == 0)
                distributionsToTest = GetAllDistributions();
            else distributionsToTest = GetDistributions(distributions);
            
            Parallel.ForEach(distributionsToTest, (dist) =>
            {
                var distribution = dist;
                try
                {
                    distribution = UnivariateDistribution(observations, distribution);

                    var R = RSquare(observations, distribution);

                    //P = new Accord.GenerateDistributions.Testing.KolmogorovSmirnovTest(observations.ToArray(),distribution, KolmogorovSmirnovTestHypothesis.SampleIsDifferent).PValue;
                    //var e =Evaluate(observations,distribution);
                    // P = Pvalue(observations.Count, e);
                    distributionsData.Add(new DistributionData(distribution, R));

                }
                catch (Exception)
                {
                    if (log != null && observations.Count <= 1)
                        log.Add(LogType.Warning,
                            "The distribution   could not be determined due to an insufficient number of observations");

                }
            });



            return distributionsData;
        }

        private static double RSquare(List<double> observations, IUnivariateDistribution distribution)
        {
            Random rand = new Random();
            var obs = observations.Distinct().ToArray();
            var newobs = new double[obs.Count()];
            double rd = 0d;


            try
            {
                if ((distribution is Accord.Statistics.Distributions.Univariate.BetaDistribution &&
                     (distribution as Accord.Statistics.Distributions.Univariate.BetaDistribution).Alpha < 0.1) ||
                    Double.IsNaN(distribution.Mean) || Double.IsNaN(distribution.Variance) ||
                    Double.IsNaN(distribution.Mode))
                    return 0;
                for (int i = 0; i < obs.Count(); i++)
                {

                    rd = rand.NextDouble();
                    newobs[i] = distribution.InverseDistributionFunction(rd);
                }
            }

            catch (ArithmeticException)
            {
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
            obs.StableSort();
            newobs.StableSort();
            var r = GoodnessOfFit.RSquared(newobs, obs);
            if (Double.IsNaN(r))
                r = 0;
            return r;
        }

        private static IUnivariateDistribution UnivariateDistribution(List<double> observations,
            IUnivariateDistribution distribution)
        {
            if (distribution is WeibullDistribution)
                distribution = MaximumLikelihoodEstimation.Weibull(observations.ToArray());
            else if (distribution is TriangularDistribution)
                distribution = MaximumLikelihoodEstimation.Triangular(observations);
            else if (distribution is BetaDistribution)
                distribution = MaximumLikelihoodEstimation.Beta(observations);
            else if (distribution is EmpiricalDistribution)
            {
                distribution = MaximumLikelihoodEstimation.Empirical(observations);
            }
            else if (distribution is GammaDistribution)
            {
                distribution = MaximumLikelihoodEstimation.Gamma(observations.ToArray());
            }
            else
                distribution?.Fit(observations.ToArray());
            return distribution;
        }


        public static double Evaluate(List<double> observations, GDMInterfaces.IDistribution dist)
        {
            if (dist == null)
                throw new Exception("Distribution not initialized");

            double dMaxMinus;
            var dMaxPlus = dMaxMinus = Double.MinValue;
            List<double> copy = observations.ToList();
            int n = copy.Count, j = 1;
            copy.Sort();

            foreach (double x in copy)
            {
                double t1 = (double) j/n - dist.CDF(x, 0, 0, 0);
                double t2 = dist.CDF(x, 0, 0, 0) - ((double) (j - 1)/n);
                if (t1 > dMaxPlus) dMaxPlus = t1;
                if (t2 > dMaxMinus) dMaxMinus = t2;
                j++;
            }

            return Math.Max(dMaxPlus, dMaxMinus);
        }

        // Reference: "Robustness and Distribution Assumptions", mathematical statistics department of CTH/GU, (?)
        public static double Pvalue(int n, double d)
        {
            double x = Math.Sqrt(n)*d, sum = 0, previousSum = Double.MinValue;
            int i = 1;

            while (Math.Abs(sum - previousSum) >= 0.000001d)
            {
                previousSum = sum;
                sum += Math.Pow(-1, i - 1)*Math.Exp(-2*i*i*x*x);
                i++;
            }

            return 2*sum;
        }
    }
}

