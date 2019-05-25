//using System;
//using System.Collections.Generic;

//namespace GenerateDistributions
//{
//    public abstract class Distribution
//    {
//        /// <summary>
//        /// The name of the distribution
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// Maximum Likelihood Estimation
//        /// </summary>
//        public abstract void Mle(List<double> observations);

//        /// <summary>
//        /// Cumulative Distribution Function
//        /// </summary>
//        public double Cdf(double x)
//        {
//            if(Dist == null)
//                    throw new Exception("Dist not initialized");
//            return Dist.CumulativeDistribution(x);
//        }

//        public double Evaluate(List<double> observations)
//        {
//            if (Dist == null)
//                throw new Exception("Distribution not initialized");

//            double dMaxMinus;
//            var dMaxPlus = dMaxMinus = Double.MinValue;
//            List<double> copy = new List<double>();
//            foreach (double x in observations) copy.Add(x);
//            int n = copy.Count, j = 1; copy.Sort();

//            foreach (double x in copy)
//            {
//                double t1 = (double)j / n - Cdf(x);
//                double t2 = Cdf(x) - ((double)(j - 1) / n);
//                if (t1 > dMaxPlus) dMaxPlus = t1;
//                if (t2 > dMaxMinus) dMaxMinus = t2;
//                j++;
//            }

//            return Math.Max(dMaxPlus, dMaxMinus);
//        }

//        // Reference: "Robustness and Distribution Assumptions", mathematical statistics department of CTH/GU, (?)
//        public double Pvalue(int n, double d)
//        {
//            double x = Math.Sqrt(n) * d, sum = 0, previousSum = Double.MinValue;
//            int i = 1;

//            while (Math.Abs(sum - previousSum) >= Tolerance)
//            {
//                previousSum = sum;
//                sum += Math.Pow(-1, i - 1) * Math.Exp(-2 * i * i * x * x);
//                i++;
//            }

//            return 2 * sum;
//        }

//        public const double Tolerance = 0.000001;
//        /// <summary>
//        /// Hook up. Used by the statistics plugin to store parameter name settings
//        /// </summary>
//        public object Tag { get; set; }


//        public double[] Samples(int n)
//        {
//            var d = new double[n];
//            if (Dist != null) Dist.Samples(d);
//            return d;
//        }
//        public abstract  MathNet.Numerics.Distributions.IContinuousDistribution Dist { get; set; }
//    }
//}
