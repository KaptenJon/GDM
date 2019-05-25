using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;

namespace GDMPlugins.Statistics
{
    public static class Miscellaneous
    {
        /// <summary>
        /// Histogram
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="intervals">Desired number of intervals</param>
        /// <param name="histogram">The histogram</param>
        /// <param name="intervalWidth">Interval width</param>
        /// <param name="minimum">Minimum observed value</param>
        /// <param name="maximum">Maximum observed value</param>
        public static void Histogram(List<double> observations, int intervals, out List<int> histogram, out int intervalWidth, out double minimum, out double maximum)
        {
            histogram = new List<int>(intervals);
            for (int i = 0; i < intervals; i++) histogram.Add(0);
            maximum = observations.Maximum(); minimum = observations.Minimum();


         

            int index; double range = maximum - minimum;
            intervalWidth = (int)Math.Ceiling(range / intervals);

            // Count occurrences
            foreach (double x in observations)
            {
                if (range != 0)
                    index = Convert.ToInt32((x - minimum) * (intervals - 1) / range);
                else index = 0;

                histogram[index]++;
            }
        }

        /// <summary>
        /// Statistical summery.
        /// </summary>
        /// <param name="observations">List with observed values</param>
        /// <param name="maximum">Maximum observed value</param>
        /// <param name="minimum">Minimum observed value</param>
        /// <param name="mean">Mean value</param>
        /// <param name="variance">Variance</param>
        /// <param name="skewness">Skewness</param>
        public static void Summary(List<double> observations, out double maximum, out double minimum, out double mean, out double variance, out double skewness)
        {
            double n = observations.Count, sum = 0, sumSquare = 0, sumTri = 0;
            maximum = Double.MinValue; minimum = Double.MaxValue;

            foreach (double x in observations)
            {
                if (x > maximum) maximum = x;
                else if (x < minimum) minimum = x;
                sum += x;
                sumSquare += x * x;
            }

            mean = sum / n;
            variance = (sumSquare - sum * mean) / (n - 1);

            foreach (double x in observations)
            {
                sumTri += Math.Pow(x - mean, 3);
            }
            skewness = sumTri / (n * Math.Pow(Math.Sqrt(variance), 3));
        }

        // Modified version of the method "GammaRegularized" in the MathNet library
        public static double ModifiedGammaRegularized(double a, double x)
        {
            const int maxIterations = 10000;    // Changed from 100
            double eps = 1.0d.EpsilonOf();
            double fpmin = Double.Epsilon / eps;

            if (a < 0.0) throw new Exception("a < 0.0");
            else if (x < 0.0) throw new Exception("x < 0.0");

            double gln = SpecialFunctions.GammaLn(a);

            if (x < a + 1.0)
            {
                if (x <= 0.0) return 0.0;
                else
                {
                    double ap = a;
                    double del, sum = del = 1.0 / a;

                    for (int n = 0; n < maxIterations; n++)
                    {
                        ++ap;
                        del *= x / ap;
                        sum += del;

                        if (Math.Abs(del) < Math.Abs(sum) * eps)
                            return sum * Math.Exp(-x + a * Math.Log(x) - gln);
                    }
                }
            }
            else
            {
                double b = x + 1.0 - a;
                double c = 1.0 / fpmin;
                double d = 1.0 / b;
                double h = d;

                for (int i = 1; i <= maxIterations; i++)
                {
                    double an = -i * (i - a);
                    b += 2.0;
                    d = an * d + b;
                    if (Math.Abs(d) < fpmin) d = fpmin;
                    c = b + an / c;
                    if (Math.Abs(c) < fpmin) c = fpmin;
                    d = 1.0 / d;
                    double del = d * c;
                    h *= del;
                    if (Math.Abs(del - 1.0) <= eps) return 1.0 - Math.Exp(-x + a * Math.Log(x) - gln) * h;
                }
            }
            throw new ArgumentException("Arument too large for iteration limit");
        }
    }
}
