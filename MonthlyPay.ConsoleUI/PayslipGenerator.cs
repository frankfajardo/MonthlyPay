using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MonthlyPay.BusinessLogic;

namespace MonthlyPay.ConsoleUI
{
    public class PayslipGenerator
    {
        private ILogger<PayslipGenerator> logger;
        private IIncomeTaxCalculator taxCalculator;
        private TextWriter outputWriter;


        public PayslipGenerator(ILogger<PayslipGenerator> logger, IIncomeTaxCalculator taxCalculator, TextWriter outputWriter)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
            this.outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        }

        public async Task GenerateMonthlyPayslipAsync(string employeeName, decimal annualIncome)
        {
            try 
            {
                var monthlyPayDetails = await taxCalculator.GetMonthlyPayDetailsAsync(annualIncome);

                outputWriter.WriteLine($"\n======================================================");
                outputWriter.WriteLine($"    Monthly Payslip for:  {Formatter.FormatName(employeeName)}");
                outputWriter.WriteLine($"------------------------------------------------------");
                outputWriter.WriteLine($"    Gross Monthly Income: {Formatter.FormatDollarAmount(monthlyPayDetails.MonthlyGrossIncome)}");
                outputWriter.WriteLine($"    Monthly Income Tax:   {Formatter.FormatDollarAmount(monthlyPayDetails.MonthlyIncomeTax)}");
                outputWriter.WriteLine($"    Net Monthly Income:   {Formatter.FormatDollarAmount(monthlyPayDetails.MonthlyNetIncome)}");
                outputWriter.WriteLine($"======================================================\n");
            }
            catch (Exception e)
            {
                logger.LogError("Payslip Generator received " + e.ToString() + ": " + e.Message + " Call stack: " + e.StackTrace);
                outputWriter.WriteLine("Error encountred whilst generating monthly payslip.");
                return;
            }
        }
    }
}
