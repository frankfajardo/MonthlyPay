using System.Threading.Tasks;

namespace MonthlyPay.BusinessLogic
{
    public interface IIncomeTaxCalculator
    {
        Task<(decimal MonthlyGrossIncome, decimal MonthlyIncomeTax, decimal MonthlyNetIncome)> GetMonthlyPayDetailsAsync(decimal annualIncome);
    }
}
