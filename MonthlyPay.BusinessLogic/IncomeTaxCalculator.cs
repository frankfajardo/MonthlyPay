using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MonthlyPay.DataStore;
using MonthlyPay.DomainModels;

namespace MonthlyPay.BusinessLogic
{
    public class IncomeTaxCalculator : IIncomeTaxCalculator
    {
        private const int MONTHS_IN_A_YEAR = 12;
        private IIncomeTaxTierRepository taxTierRepository;
        private List<IIncomeTaxTier> incomeTaxTiers = new List<IIncomeTaxTier>();

        public IncomeTaxCalculator(IIncomeTaxTierRepository taxTierRepository)
        {
            this.taxTierRepository = taxTierRepository ?? throw new ArgumentNullException(nameof(taxTierRepository));
        }

        public async Task<(decimal monthlyGrossIncome, decimal monthlyIncomeTax, decimal monthlyNetIncome)> GetMonthlyPayDetailsAsync(decimal annualIncome)
        {
            var annualTax = await GetAnnualIncomeTax(annualIncome);
            var monthlyGrossIncome = decimal.Round(annualIncome / MONTHS_IN_A_YEAR, 2);
            var monthlyIncomeTax = decimal.Round(annualTax / MONTHS_IN_A_YEAR, 2);
            var monthlyNetIncome = monthlyGrossIncome - monthlyIncomeTax;
            return (monthlyGrossIncome, monthlyIncomeTax, monthlyNetIncome);
        }

        private async Task<decimal> GetAnnualIncomeTax(decimal annualIncome)
        {
            if (!incomeTaxTiers.Any())
            {
                incomeTaxTiers = await taxTierRepository.GetIncomeTaxTiersAsync();
            }

            return incomeTaxTiers
                .Where((tier) => tier.TierLowestDollarAmount <= annualIncome)
                .Select((tier) => 
                    (Math.Min(annualIncome, (tier.TierHighestDollarAmmount ?? decimal.MaxValue)) - (tier.TierLowestDollarAmount - 1m))
                     * tier.TierRate)
                .Sum();
        }
    }
}
