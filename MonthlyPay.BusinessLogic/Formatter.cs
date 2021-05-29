namespace MonthlyPay.BusinessLogic
{
    public static class Formatter
    {
        public static string FormatName(string name)
        {
            return $"{name,24}";
        }

        public static string FormatDollarAmount(decimal amount)
        {
            return $"{amount,24:C2}";
        }
    }
}
