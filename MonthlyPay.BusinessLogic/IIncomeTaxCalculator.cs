using System.Threading.Tasks;

namespace MonthlyPay.BusinessLogic
{
    public interface IIncomeTaxCalculator
    {
        Task<(decimal monthlyGrossIncome, decimal monthlyIncomeTax, decimal monthlyNetIncome)> GetMonthlyPayDetailsAsync(decimal annualIncome);
    }
}
