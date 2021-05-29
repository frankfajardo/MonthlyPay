using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using MonthlyPay.DataStore;
using MonthlyPay.DomainModels;

using Moq;

using Xunit;

namespace MonthlyPay.BusinessLogic.Tests
{
    public class IncomeTaxCalculatorTests
    {
        private Mock<IIncomeTaxTierRepository> taxTierRepositoryMock;
        private IncomeTaxCalculator sut;

        public IncomeTaxCalculatorTests()
        {
            taxTierRepositoryMock = new Mock<IIncomeTaxTierRepository>();

            sut = new IncomeTaxCalculator(taxTierRepositoryMock.Object);

            // Arrange tax tier for all tests
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
            taxTierRepositoryMock.Setup(r => r.GetIncomeTaxTiersAsync())
                .Returns(Task.FromResult(incomeTaxTiers));
        }

        public static IEnumerable<object[]> MonthlyPayCalculationData()
        {
            return new List<SampleData>
            {
                new SampleData { AnnualIncome = 20000m, ExpectedMonthlyGross = 1666.67m, ExpectedMonthlyTax = 0m, ExpectedMonthlyNet = 1666.67m },
                new SampleData { AnnualIncome = 30000m, ExpectedMonthlyGross = 2500m, ExpectedMonthlyTax = 83.33m, ExpectedMonthlyNet = 2416.67m },
                new SampleData { AnnualIncome = 40000m, ExpectedMonthlyGross = 3333.33m, ExpectedMonthlyTax = 166.67m, ExpectedMonthlyNet = 3166.66m },
                new SampleData { AnnualIncome = 80000m, ExpectedMonthlyGross = 6666.67m, ExpectedMonthlyTax = 833.33m, ExpectedMonthlyNet = 5833.34m },
                new SampleData { AnnualIncome = 100000m, ExpectedMonthlyGross = 8333.33m, ExpectedMonthlyTax = 1333.33m, ExpectedMonthlyNet = 7000m },
                new SampleData { AnnualIncome = 180000m, ExpectedMonthlyGross = 15000m, ExpectedMonthlyTax = 3333.33m, ExpectedMonthlyNet = 11666.67m },
                new SampleData { AnnualIncome = 250000m, ExpectedMonthlyGross = 20833.33m, ExpectedMonthlyTax = 5666.67m, ExpectedMonthlyNet = 15166.66m },
            }
            .Select((sample) => new object[] { sample });
        }

        [Theory]
        [MemberData(nameof(MonthlyPayCalculationData))]
        public void GetMonthlyPayDetailsAsync_ShouldReturnCorrectMonthylPayslipDetails(SampleData sample)
        {
            // Act
            var result = sut.GetMonthlyPayDetailsAsync(sample.AnnualIncome).GetAwaiter().GetResult();

            // Assert
            result.MonthlyGrossIncome.Should().Be(sample.ExpectedMonthlyGross);
            result.MonthlyIncomeTax.Should().Be(sample.ExpectedMonthlyTax);
            result.MonthlyNetIncome.Should().Be(sample.ExpectedMonthlyNet);
        }

    }
    public class SampleData
    {
        public decimal AnnualIncome { get; set; }
        public decimal ExpectedMonthlyGross { get; set; }
        public decimal ExpectedMonthlyTax { get; set; }
        public decimal ExpectedMonthlyNet { get; set; }
    }
}
