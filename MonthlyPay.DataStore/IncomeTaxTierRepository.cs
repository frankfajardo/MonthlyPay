using System.Collections.Generic;
using System.Threading.Tasks;

using MonthlyPay.DomainModels;

namespace MonthlyPay.DataStore
{
    /// <summary>
    /// Represents a repository for income tax tiers
    /// </summary>
    public class IncomeTaxTierRepository : IIncomeTaxTierRepository
    {
        /// <summary>
        /// Retrieves all income tax tiers
        /// </summary>
        /// <returns></returns>
        public Task<List<IIncomeTaxTier>> GetIncomeTaxTiersAsync()
        {
            // To simplify the solution and leave out the complexity of a database,
            // this repository is simply returning hard-coded data.
            // Ideally, this would be retrieved from an actual database or any location that 
            // is independent of the application source code, as this data is likely to change
            // more frequently.
            var incomeTaxTiers = new List<IIncomeTaxTier>()
                {
                    new IncomeTaxTier() {
                        TierLowestDollarAmount = 0m,
                        TierHighestDollarAmmount = 20000m,
                        TierRate = 0,
                    },
                    new IncomeTaxTier() {
                        TierLowestDollarAmount = 20001m,
                        TierHighestDollarAmmount = 40000m,
                        TierRate = 0.1m,
                    },
                    new IncomeTaxTier() {
                        TierLowestDollarAmount = 40001m,
                        TierHighestDollarAmmount = 80000m,
                        TierRate = 0.2m,
                    },
                    new IncomeTaxTier() {
                        TierLowestDollarAmount = 80001m,
                        TierHighestDollarAmmount = 180000m,
                        TierRate = 0.3m,
                    },
                    new IncomeTaxTier() {
                        TierLowestDollarAmount = 180001m,
                        TierHighestDollarAmmount = null,
                        TierRate = 0.4m,
                    }
                };
            return Task.FromResult(incomeTaxTiers);
        }
    }

    public class IncomeTaxTier : IIncomeTaxTier
    {
        public decimal TierLowestDollarAmount { get; set; }
        public decimal? TierHighestDollarAmmount { get; set; }
        public decimal TierRate { get; set; }
    }
}
