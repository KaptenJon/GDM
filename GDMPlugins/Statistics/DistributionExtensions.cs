using System;
using Accord;
using Accord.Math.Optimization;
using Accord.Statistics;
using Accord.Statistics.Distributions;
using Accord.Statistics.Distributions.Fitting;

namespace GDMPlugins.Statistics
{

    public static class DistributionParser
    {
        public static IUnivariateDistribution Parse(string distributionString)
        {
            if (distributionString.StartsWith("Weibull("))
            {
                var par = GetParameters(distributionString, "Weibull(");
                return new Accord.Statistics.Distributions.Univariate.WeibullDistribution(par[0], par[1]);
            }
            else if (distributionString.StartsWith("Normal("))
            {

            }
            else if (distributionString.StartsWith("Gamma("))
            {

            }
            else if (distributionString.StartsWith("Exponential("))
            {

            }
            else if (distributionString.StartsWith("Lognormal("))
            {

            }
            else if (distributionString.StartsWith("Triangular("))
            {

            }
            else if (distributionString.StartsWith("Uniform("))
            {

            }
            else if (distributionString.StartsWith("Beta("))
            {

            }
            else if (distributionString.StartsWith("Empirical("))
            {

            }
            return null;
        }

        private static double[] GetParameters(string distributionString, string distname)
        {
            var para = distributionString.Remove(0, distname.Length).Remove(')').Split(';');
            var ret = new double[para.Length];
            foreach (var s in para)
            {
               
            }
            return ret;

        }
    }

    public class WeibullDistribution : Accord.Statistics.Distributions.Univariate.WeibullDistribution
    {
        public override string ToString()
        {
            return base.ToString().Replace("x; ", "");
        }

        public WeibullDistribution(double shape, double scale) : base(shape, scale)
        {
        }

        
    }
   
    public class NormalDistribution : Accord.Statistics.Distributions.Univariate.NormalDistribution
    {
        public override string ToString()
        {
            return base.ToString().Replace("x; ", "").Replace("N", "Normal");
        }
    }

    public class GammaDistribution : Accord.Statistics.Distributions.Univariate.GammaDistribution
    {
        public GammaDistribution( double theta, double k) : base(theta, k)
        {
        }
        public GammaDistribution() : base()
        {
        }
        public override string ToString()
        {
            return base.ToString().Replace("x; ", "").Replace("Γ", "Gamma");
        }
    }

    public class ExponentialDistribution : Accord.Statistics.Distributions.Univariate.ExponentialDistribution
    {

        public override string ToString()
        {
            return base.ToString().Replace("Exp(x; ", "Exponential(");
        }
    }

    public class LognormalDistribution : Accord.Statistics.Distributions.Univariate.LognormalDistribution
    {
        public override string ToString()
        {
          
            return base.ToString().Replace("x; ", "");
        }
    }

    public class TriangularDistribution : Accord.Statistics.Distributions.Univariate.TriangularDistribution
    {
        public override string ToString()
        {
            return base.ToString().Replace("x; ", "");
        }

        public TriangularDistribution(double min, double max, double mode) : base(min, max, mode)
        {
        }
    }

    public class UniformContinuousDistribution :
        Accord.Statistics.Distributions.Univariate.UniformContinuousDistribution
    {
        public override string ToString()
        {
            return base.ToString().Replace("U(x; ", "Uniform(");
        }
    }

    public class EmpiricalDistribution : Accord.Statistics.Distributions.Univariate.EmpiricalDistribution
    {
        public override string ToString()
        {
            string ret = "Empirical(";
            for (int index = 0; index < Samples.Length; index++)
            {
                var sample = Samples[index];
                ret += sample + "|" + Weights[index] + " ;";
            }

            return ret.TrimEnd(';') + ")";
        }

        public EmpiricalDistribution(double[] samples) : base(samples)
        {
        }

        public EmpiricalDistribution(double[] samples, double smoothing) : base(samples, smoothing)
        {
        }

        public EmpiricalDistribution(double[] samples, double[] weights) : base(samples, weights)
        {
        }

        public EmpiricalDistribution(double[] samples, int[] weights) : base(samples, weights)
        {
        }

        public EmpiricalDistribution(double[] samples, double[] weights, double smoothing)
            : base(samples, weights, smoothing)
        {
        }

        public EmpiricalDistribution(double[] samples, int[] weights, double smoothing)
            : base(samples, weights, smoothing)
        {
        }
    }

    public class BetaDistribution : Accord.Statistics.Distributions.Univariate.BetaDistribution
    {
        public BetaDistribution(double alpha, double beta) : base(alpha, beta)
        {
        }

        public BetaDistribution() : base()
        {
        }

        public override string ToString()
        {
            return base.ToString().Replace("B(x; ", "Beta(");
        }

        public new static BetaDistribution Estimate(double[] observations)
        {

            //LibOptimization.

            double mean;
            double var;



            mean = observations.Mean();
            var = observations.Variance(mean);

            double sum1 = 0, sum2 = 0;
            for (int i = 0; i < observations.Length; i++)
            {
                sum1 += Math.Log(observations[i]);
                sum2 += Math.Log(1 - observations[i]);
            }


            double[] gradient = new double[2];


            var bfgs = new BoundedBroydenFletcherGoldfarbShanno(numberOfVariables: 2);
            bfgs.LowerBounds[0] = 1e-100;
            bfgs.LowerBounds[1] = 1e-100;
            //if (var >= mean*(1.0 - mean))
            //    throw new NotSupportedException();


            double u = (mean*(1 - mean)/var) - 1.0;
            double alpha = mean*u;
            double beta = (1 - mean)*u;

            bfgs.Solution[0] = alpha;
            bfgs.Solution[1] = beta;


            bfgs.Function = (double[] parameters) =>
                LogLikelihood(sum1, sum2, observations.Length, parameters[0], parameters[1]);


            bfgs.Gradient = (double[] parameters) =>
                Gradient(sum1, sum2, observations.Length, parameters[0], parameters[1], gradient);


            if (!bfgs.Minimize())
                throw new ConvergenceException();


            alpha = bfgs.Solution[0];
            beta = bfgs.Solution[1];
            return new BetaDistribution(alpha,beta);
        }
    }
}




