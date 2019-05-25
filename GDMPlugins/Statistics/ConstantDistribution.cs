using System;
using System.Linq;
using Accord;
using Accord.Statistics.Distributions.Fitting;



namespace GDMPlugins.Statistics
{
    public class ConstantDistribution : Accord.Statistics.Distributions.IUnivariateDistribution
    {
        private double _mean;

        private const double Tolerance = 0.00001;

        public override string ToString()
        {
            return "Constant("+_mean + ")";
        }

        public object Clone()
        {
            return null;
        }

        public double DistributionFunction(params double[] x)
        {
            return DistributionFunction(x[0]);
        }

        public double ProbabilityFunction(params double[] x)
        {
            return ProbabilityFunction(x[0]);
        }

        public double LogProbabilityFunction(params double[] x)
        {
            return LogProbabilityFunction(x[0]);
        }

        public double ComplementaryDistributionFunction(params double[] x)
        {
            return ComplementaryDistributionFunction(x[0]);
        }


        public void Fit(Array observations)
        {
            _mean = observations.Cast<double>().Average();
        }
    


        public void Fit(Array observations, double[] weights)
        {
            var obs = observations.Cast<double>().ToArray();
            var sum = 0d;
            var weightsSum = 0d;
            for (int i = 0; i < obs.Count(); i++)
            {
                sum += obs[i]*weights[i];
                weightsSum += weights[i];
            }
            _mean = sum/weightsSum;
        }

        public void Fit(Array observations, int[] weights)
        {
            Fit(observations,  weights.Cast<double>().ToArray());
        }

        public void Fit(Array observations, IFittingOptions options)
        {
            Fit(observations);
        }

        public void Fit(Array observations, double[] weights, IFittingOptions options)
        {
            Fit(observations, weights);
        }

        public void Fit(Array observations, int[] weights, IFittingOptions options)
        {
            Fit(observations, weights);
        }

        public DoubleRange GetRange(double percentile)
        {
            return new DoubleRange(_mean,_mean);
        }

        public double DistributionFunction(double x)
        {
            return x < _mean ? 0 : 1;
        }

        public double DistributionFunction(double a, double b)
        {
            return DistributionFunction(a);
        }

        public double ProbabilityFunction(double x)
        {
            return Math.Abs(x - _mean) < Tolerance ? 0 : 1;
        }
        

        public double LogProbabilityFunction(double x)
        {
            return Math.Abs(x - _mean) < Tolerance ? 0 : 1;
        }

        public double InverseDistributionFunction(double p)
        {
            return _mean;
        }

        public double ComplementaryDistributionFunction(double x)
        {
            return 1 - DistributionFunction(x); ;
        }

        public double HazardFunction(double x)
        {
            return 0;
        }

        public double CumulativeHazardFunction(double x)
        {
            return 0;
        }

        public double LogCumulativeHazardFunction(double x)
        {
            return 0;
        }

        public double QuantileDensityFunction(double p)
        {
            return 0;
        }

        public double Mean => _mean;

        public double Variance => 0;

        public double Median => _mean;

        public double Mode => _mean;

        public double Entropy => 0;

        public DoubleRange Support => new DoubleRange();

        public DoubleRange Quartiles => new DoubleRange();

      
    }
}
