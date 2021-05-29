namespace MonthlyPay.DomainModels
{
    public interface IIncomeTaxTier
    {
        /// <summary>
        /// The lowest dollar amount for this tier
        /// </summary>
        decimal TierLowestDollarAmount { get; set; }
        /// <summary>
        /// The highest dollar amount for this tier. Set to null for the top-most tier
        /// </summary>
        decimal? TierHighestDollarAmmount { get; set; }
        /// <summary>
        /// The applicable tier rate per dollar amount
        /// </summary>
        decimal TierRate { get; set; }
    }
}
