using System;

using Moq;

using Xunit;
using Microsoft.Extensions.Logging;
using MonthlyPay.BusinessLogic;
using System.IO;
using FluentAssertions;

namespace MonthlyPay.ConsoleUI.Tests
{
    public class PayslipGeneratorTests
    {
        private Mock<ILogger<PayslipGenerator>> loggerMock;
        private Mock<IIncomeTaxCalculator> taxCalculatorMock;
        private PayslipGenerator sut;
        private StringWriter outputWriter;

        public PayslipGeneratorTests()
        {
            loggerMock = new Mock<ILogger<PayslipGenerator>>();
            taxCalculatorMock = new Mock<IIncomeTaxCalculator>();
            outputWriter = new StringWriter();

            sut = new PayslipGenerator(loggerMock.Object, taxCalculatorMock.Object, outputWriter);
        }

        [Fact]
        public void GenerateMonthlyPayslipAsync_WhenSuccessful_ShouldOutputRequiredDetails()
        {
            var monthlyTaxDetails = (MonthlyGrossIncome: 5000m, MonthlyIncomeTax: 500m, MonthlyNetIncome: 4500m);
            taxCalculatorMock.Setup(c => c.GetMonthlyPayDetailsAsync(It.IsAny<decimal>()))
                .ReturnsAsync(monthlyTaxDetails);

            sut.GenerateMonthlyPayslipAsync("John Doe", 60000m).GetAwaiter().GetResult();

            var result = outputWriter.ToString();
            result.Should().Contain("Monthly Payslip for:                  John Doe");
            result.Should().Contain("Gross Monthly Income:                $5,000.00");
            result.Should().Contain("Monthly Income Tax:                    $500.00");
            result.Should().Contain("Net Monthly Income:                  $4,500.00");
        }

        [Fact]
        public void GenerateMonthlyPayslipAsync_WhenCalculatorFails_ShouldNotThrowException()
        {
            taxCalculatorMock.Setup(c => c.GetMonthlyPayDetailsAsync(It.IsAny<decimal>()))
                .Throws(new InvalidOperationException("Some invalid operation"));

            Action act = () => sut.GenerateMonthlyPayslipAsync("John Doe", 120000m).GetAwaiter().GetResult();

            act.Should().NotThrow<Exception>();
        }

        [Fact]
        public void GenerateMonthlyPayslipAsync_WhenCalculatorFails_ShouldOutputFriendlyError()
        {
            taxCalculatorMock.Setup(c => c.GetMonthlyPayDetailsAsync(It.IsAny<decimal>()))
                .Throws(new InvalidOperationException("Some invalid operation"));

            sut.GenerateMonthlyPayslipAsync("John Doe", 60000m).GetAwaiter().GetResult();

            var friendlyErrorMessage = "Error encountred whilst generating monthly payslip." + Environment.NewLine;
            var result = outputWriter.ToString();
            result.Should().Be(friendlyErrorMessage);
        }
    }
}
