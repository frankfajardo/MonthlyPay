using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MonthlyPay.BusinessLogic;
using MonthlyPay.DataStore;

namespace MonthlyPay.ConsoleUI
{
    class Program
    {
        static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            string employeeName = args?.Length >= 1 ? args[0] : null;
            if (employeeName is null || args.Length < 2 || !decimal.TryParse(args[1], out decimal annualSalary))
            {
                showHowToRun();
                return;
            }

            // Load configurations
            BuildConfigurations();

            // Build our service provider
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Run our payslip generator
            serviceProvider.GetService<PayslipGenerator>().GenerateMonthlyPayslipAsync(employeeName, annualSalary).GetAwaiter().GetResult();

            // If there is a 3rd argument, pause until a key is pressed on the screen
            if (args.Length >= 3)
            {
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        private static void showHowToRun()
        {
            Console.WriteLine("Please run command with the employee name and salary, as follow:\nGenerateMonthlyPayslip \"Jane\" 140000");
        }

        private static void BuildConfigurations()
        {
            string environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                // No environment settings are used on this project, but this is a good practice.
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
            // Build all our configuration into a single Configuration object.
            Configuration = builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Configure logger and add to services
            var loggerFactory = new LoggerFactory();
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            services.AddSingleton<IIncomeTaxTierRepository, IncomeTaxTierRepository>();
            services.AddSingleton<IIncomeTaxCalculator, IncomeTaxCalculator>();

            // Add console as target for App output
            services.AddSingleton(Console.Out);

            // Add our payslip generator
            services.AddSingleton<PayslipGenerator>();

        }
    }
}
