using Accord.Statistics.Distributions;

namespace GDMPlugins.Statistics
{
    public class DistributionData
    {
        public IDistribution Distribution;
        public double RSquare;


        public DistributionData(IDistribution distribution, double rSquare)
        {
            Distribution = distribution;
            RSquare = rSquare;

        }

    }
}
