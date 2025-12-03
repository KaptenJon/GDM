using System.Collections.Generic;

namespace GDMInterfaces
{
    public interface IDistribution
    {
        /// <summary>
        /// The name of the distribution
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Maximum Likelihood Estimation
        /// </summary>
        /// <param name="a">Location parameter</param>
        /// <param name="b">Scale parameter</param>
        /// <param name="c">Shape parameter</param>
        void MLE(List<double> observations, out double a, out double b, out double c);

        /// <summary>
        /// Cumulative Distribution Function
        /// </summary>
        double CDF(double x, double a, double b, double c);

        /// <summary>
        /// Method that samples the distribution
        /// </summary>
        double Sample(double a, double b, double c);

        /// <summary>
        /// Distribution mean
        /// </summary>
        /// <returns>Mean</returns>
        double Mean(double a, double b, double c);

        /// <summary>
        /// Distribution variance
        /// </summary>
        /// <returns>Variance</returns>
        double Variance(double a, double b, double c);

        /// <summary>
        /// Hook up. Used by the statistics plugin to store parameter name settings
        /// </summary>
        object Tag { get; set; }
    }
}
