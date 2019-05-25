//using System;
//using System.Collections.Generic;

//namespace GDMPlugins
//{
//    internal class Distributions
//    {
//        internal class Weibull : IDistribution
//        {
//            private object tag;

//            public string Name
//            {
//                get { return "Weibull"; }
//            }

//            public void MLE(List<double> observations, out double a, out double b, out double c)
//            {
//                a = 0.0;
//                MaximumLikelihoodEstimation.Weibull(observations, out b, out c);
//            }

//            public double CDF(double x, double a, double b, double c)
//            {
//                return CumulativeDistributionFunction.Weibull(x, b, c);
//            }

//            public double Mean(double a, double b, double c)
//            {
//                return b * Miscellaneous.GammaFunction(1 + 1 / c);
//            }

//            public double Variance(double a, double b, double c)
//            {
//                return b * b * Miscellaneous.GammaFunction(1 + 2 / c) - Math.Pow(this.Mean(a, b, c), 2);
//            }

//            public object Tag
//            {
//                get { return this.tag; }
//                set { this.tag = value; }
//            }

//            public double Sample(double a, double b, double c)
//            {
//                return DistributionSample.Weibull(a, b, c);
//            }
//        }

//        internal class LogNormal : IDistribution
//        {
//            private object tag;

//            public string Name
//            {
//                get { return "LogNormal"; }
//            }

//            public void MLE(List<double> observations, out double a, out double b, out double c)
//            {
//                a = 0.0;
//                MaximumLikelihoodEstimation.LogNormal(observations, out b, out c);
//            }

//            public double CDF(double x, double a, double b, double c)
//            {
//                return CumulativeDistributionFunction.LogNormal(x, b, c);
//            }

//            public double Mean(double a, double b, double c)
//            {
//                return Math.Exp(b + (c * c) / 2);
//            }

//            public double Variance(double a, double b, double c)
//            {
//                return (Math.Exp(c * c) - 1) * Math.Exp(2 * b + c * c);
//            }

//            public object Tag
//            {
//                get { return this.tag; }
//                set { this.tag = value; }
//            }

//            public double Sample(double a, double b, double c)
//            {
//                return DistributionSample.LogNormal(a, b, c);
//            }
//        }

//        internal class Exponential : IDistribution
//        {
//            private object tag;

//            public string Name
//            {
//                get { return "Exponential"; }
//            }

//            public void MLE(List<double> observations, out double a, out double b, out double c)
//            {
//                a = 0.0;
//                MaximumLikelihoodEstimation.Exponential(observations, out b);
//                c = 0.0;
//            }

//            public double CDF(double x, double a, double b, double c)
//            {
//                return CumulativeDistributionFunction.Exponential(x, b);
//            }

//            public double Mean(double a, double b, double c)
//            {
//                return b;
//            }

//            public double Variance(double a, double b, double c)
//            {
//                return b * b;
//            }

//            public object Tag
//            {
//                get { return this.tag; }
//                set { this.tag = value; }
//            }

//            public double Sample(double a, double b, double c)
//            {
//                return DistributionSample.Exponential(a, b, c);
//            }
//        }

//        internal class Gamma : IDistribution
//        {
//            private object tag;

//            public string Name
//            {
//                get { return "Gamma"; }
//            }

//            public void MLE(List<double> observations, out double a, out double b, out double c)
//            {
//                a = 0.0;
//                MaximumLikelihoodEstimation.Gamma(observations, out b, out c);
//            }

//            public double CDF(double x, double a, double b, double c)
//            {
//                return CumulativeDistributionFunction.Gamma(x, b, c);
//            }

//            public double Mean(double a, double b, double c)
//            {
//                return b * c;
//            }

//            public double Variance(double a, double b, double c)
//            {
//                return c * b * b;
//            }

//            public object Tag
//            {
//                get { return this.tag; }
//                set { this.tag = value; }
//            }

//            public double Sample(double a, double b, double c)
//            {
//                return DistributionSample.Gamma(a, b, c);
//            }
//        }
//    }
//}
