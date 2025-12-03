using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Statistics.Distributions;
using LibOptimization.Optimization;
using MathNet.Numerics.Statistics;
using Meta.Numerics.Statistics;

namespace GDMPlugins.Statistics
{
    public static class MaximumLikelihoodEstimation
    {
        static TaskFactory<double> tasks = new TaskFactory<double>();
        /// <summary>
        /// Weibull distribution.
        /// Reference: "Parameter estimation of the Weibull probability distribution", Hongzhu Qiao, Chris P. Tsokos (1994)
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="lout">Scale parameter beta</param>
        /// <param name="kout">Shape parameter alpha</param>
        public static void Weibull(List<double> observations, out double lout, out double kout)
        {
            var obs = observations.Where(t => t > 0);//.Select( i => Convert.ToDecimal(i));
            var observationsorted = obs as double[] ?? obs.ToArray();
            //var observationsorted2 = obs as double[] ?? obs.ToArray();
            //var observationsorted3 = obs as double[] ?? obs.ToArray();
            double n = observationsorted.Count(), qofk = 0, abs = 1;

            if (n <= 1) throw new Exception("MLE error: number of observations not sufficient");

            // Start values
            double k = 1;
            int test2 = 0;
            while (abs >= 0.00000000001d)
            {
                test2++;
                var t1 = tasks.StartNew(() =>
                {
                    return Enumerable.Sum(observationsorted.AsParallel(), x => Math.Log(x));
                });
                var t2 = tasks.StartNew(() =>
                {
                    return Enumerable.Sum(observationsorted.AsParallel(), x => Math.Pow(x, k));
                });
                var t3 = tasks.StartNew(() =>
                {
                    return Enumerable.Sum(observationsorted.AsParallel(), x => Math.Pow(x, k) * Math.Log(x));
                });
                Task.WaitAll(t1,t2,t3);
                
                k = qofk;
                qofk = 1/((t3.Result/t2.Result) - t1.Result/n);
                //qofC = n * s2 / (n * s3 - s1 * s2);

                //previousk = k;
                k = (k + qofk) / 2;
                abs = k < qofk ? qofk - k : k - qofk;
            }

            double s4 = 0;
            s4 = observationsorted.Sum(x => Math.Pow(x, k));
            s4 = s4/n;
            lout = Math.Pow(s4,1/k);
            kout = k;
            
        }

        public static GammaDistribution Gamma(double[] observations)
        {
            
            double lnsum = 0;
            for (int i = 0; i < observations.Length; i++)
                lnsum += Math.Log(observations[i]);

            double mean = observations.Mean();

            double s = Math.Log(mean) - lnsum / observations.Length;

            if (Double.IsNaN(s))
                throw new ArgumentException("Observation vector contains negative values.", "observations");

            // initial approximation
            double newK = (3 - s + Math.Sqrt((s - 3) * (s - 3) + 24 * s)) / (12 * s);

            // Use Newton-Raphson approximation
            double oldK;
            double epsilon = 0;
            do
            {
                oldK = newK;
                newK = oldK - (Math.Log(newK) - Accord.Math.Gamma.Digamma(newK) - s) / ((1 / newK) - Accord.Math.Gamma.Trigamma(newK));
                epsilon = Math.Max(Math.Abs(newK) * 1E-14, Math.Abs(oldK)) * 1E-14;
            }
          while (Math.Abs(oldK - newK) > epsilon && Math.Abs(oldK - newK) / Math.Abs(oldK) > epsilon);
            
            double theta = mean / newK;
            if (Double.IsNaN(theta))
                return new GammaDistribution();

            return new GammaDistribution(theta, newK);
        }

        /// <summary>
        /// Weibull distribution.
        /// Reference: "Parameter estimation of the Weibull probability distribution", Hongzhu Qiao, Chris P. Tsokos (1994)
        /// </summary>
        /// <param name="observations">List with observed values</param>
        public static IUnivariateDistribution Weibull(IEnumerable<double> observations)
        { 
            //var obs = observations.ToArray();
            //var w = MathNet.Numerics.Distributions.Weibull.Estimate(obs);
            //WeibullDistribution d = new WeibullDistribution(1,1);
            //d.Fit(obs);
            //return d;
            var obs = observations.Where(t=>t>0).OrderBy(t=>t).ToArray();
            var s = new Sample(obs);
            var fr = Meta.Numerics.Statistics.Distributions.WeibullDistribution.FitToSample(s);

            return new WeibullDistribution(fr.Parameters[1].Estimate.Value, fr.Parameters[0].Estimate.Value);
            

           
        }
        internal class MyClass: absObjectiveFunction
        {
            

            private List<double> _obs;

            public MyClass(IEnumerable<double> observations)
            {
            _obs = observations as List<double> ?? observations.ToList();
        }

            public override int NumberOfVariable()
            {
                return _numberOfVariable;
            }

            public override double F(List<double> x)
            {
                if (x[0] <= 0 || x[1] <= 0)
                    return Double.MaxValue;
                var dist = new WeibullDistribution(x[0], x[1]);
                return DistributionEvaluator.Evaluate(_obs, dist);
            }
            public WeibullDistribution GetOptimalWeibull()
            {
                var obt = new LibOptimization.Optimization.clsOptRealGABLX(this);
                obt.Init();

                obt.DoIteration();
                return new WeibullDistribution(obt.Result[0], obt.Result[1]);
        }

            public int _numberOfVariable = 2;
        }
        public static IUnivariateDistribution Triangular(List<double> observations)
        {
            var mode = (observations.Mean()*3);
            mode-= observations.Max() + observations.Min();
            if (mode < observations.Min())
                mode = observations.Min();
            else if (mode > observations.Max())
                mode = observations.Max();
            return new TriangularDistribution(observations.Min(), observations.Max(), mode);
        }

        public static IUnivariateDistribution Beta(List<double> observations)
        {
            var mean = observations.Mean();
            var variance = observations.Variance();
            var alpha = mean*(((mean*(1 - mean))/variance) - 1);
            var beta = (1 - mean)*(((mean*(1 - mean))/variance) - 1);
           // var betad = new Accord.GenerateDistributions.Distributions.Univariate.BetaDistribution();
            //betad.Fit(observations.ToArray(), );
            return BetaDistribution.Estimate(observations.ToArray());
            //return new BetaDistribution(alpha,beta);
        }

        public static IUnivariateDistribution Empirical(List<double> observations)
        {
            
            var steps = new List<double>();

            if (observations == null || !observations.Any())
                return null;
            var obs = observations.OrderBy(t => t).ToArray();
            
             var nrofsteps = Math.Min(observations.Count / 2, 10);
            
            var min = obs.Min();
            var max = obs.Max();
            int stepsize = obs.Count()/(nrofsteps-1);
            int step = 0;
            
            while (step < observations.Count)
            {
                steps.Add(obs[step]);
                step += stepsize;
            }
            return new EmpiricalDistribution(steps.ToArray(),0);
        }


        /// <summary>
        /// Exponential distribution.
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="delta">Scale parameter</param>
        public static void Exponential(List<double> observations, out double delta)
        {
            if (observations.Count <= 1) throw new Exception("MLE error: number of observations not sufficient");

            delta = observations.Where(x => x > 0).Average();

        }
        
        /// <summary>
        /// LogNormal distribution.
        /// Reference: "Estimation of parameters of a lognormal distribution", Wei-Hsiung Shen (1998)
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="alpha">Scale parameter</param>
        /// <param name="beta">Shape parameter</param>
        [Obsolete("Move to MathNET")]
        public static void LogNormal(List<double> observations, out double alpha, out double beta)
        {
            double n = observations.Count, sum = 0, y, sSquare, theta, etaSquare;

            if (n <= 1) throw new Exception("MLE error: number of observations not sufficient");

            foreach (double x in observations)
            {
                if (x > 0) sum += Math.Log(x);
            }
            y = sum / n;

            sum = 0;
            foreach (double x in observations)
            {
                if (x > 0) sum += Math.Pow((Math.Log(x) - y), 2);
            }
            sSquare = sum;

            theta = Math.Pow(Math.E, y) * GofT(sSquare / (2 * n), n);
            etaSquare = Math.Pow(Math.E, 2 * y) * (GofT((2 * sSquare) / n, n) - GofT((sSquare * (n - 2)) / (n * (n - 1)), n));

            alpha = Math.Log(theta) - Math.Log(etaSquare / (theta * theta) + 1) / 2;
            beta = Math.Sqrt(2 * Math.Log(theta) - 2 * alpha);
        }

        /// <summary>
        /// Gamma distribution.
        /// Reference: "Estimating a Gamma distribution", Thomas P. Minka (2002)
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="b">Scale parameter</param>
        /// <param name="a">Shape parameter</param>
        public static void Gamma(List<double> observations, out double b, out double a)
        {
            

            double sum = 0, logSum = 0, n = observations.Count, previousA = Double.MinValue;

            if (n <= 1) throw new Exception("MLE error: number of observations not sufficient");
            if (observations.StandardDeviation() <= Tolerance) throw new Exception("MLE error: to low sample variation");

            foreach (double x in observations)
            {
                if (x > 0)
                {
                    sum += x;
                    logSum += Math.Log(x);
                }
            }
            //logSum = Math.Abs(logSum);
            // Start value
            a = 0.5 / (Math.Log(sum / n) - (logSum / n));
            
            while (Math.Abs(previousA - a) >= Tolerance)
            {
                previousA = a;
                a = 1 / (1 / a + (logSum / n - Math.Log(sum / n) + Math.Log(a) - PhiOfA(a)) / (a - a * a * PhiPrimOfA(a)));
            }

            b = (sum / n) / a;
        }

        // --------------------------- Private stuff follows --------------------------- //

        private const double Tolerance = 0.00001;

        // Help method for Gamma
        private static double PhiOfA(double a)
        {
            if (Math.Abs(a) < Tolerance)
                return 0;
            if (a >= 8)
                return Math.Log(a) - (1d + (1d - (1d / 10d - 1d / (21d * a * a)) / (a * a)) / (6d * a)) / (2d * a);
            else
                return PhiOfA(a + 1d) - (1d / a);
        }

        // Help method for Gamma
        private static double PhiPrimOfA(double a)
        {
            if (a >= 8)
                return (1 + (1 + (1 - (1 / 5d - 1 / (7 * a * a)) / (a * a)) / (3 * a)) / (2 * a)) / a;
            else
                return PhiPrimOfA(a + 1) + (1 / (a * a));
        }

        // Help method for LogNormal
        private static double GofT(double t, double n)
        {
            double a = 1, sum = 1 + t, term = Double.MaxValue;
            int i = 2, step = 1;

            while (term >= Tolerance)
            {
                a *= (n - 1) / (n + step);
                term = a * Math.Pow(t, i) / Factorial(i);
                sum += term;
                step += 2;
                i++;
            }
            return sum;
        }

        // Help method for LogNormal
        private static double Factorial(double n)
        {
            if (n <= 1) return 1;
            else return n * Factorial(n - 1);
        }
    }
}
