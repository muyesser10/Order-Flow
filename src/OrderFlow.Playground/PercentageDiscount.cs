class PercentageDiscount : DiscountBase
{
    public decimal Rate { get; }

    public PercentageDiscount(decimal rate)
    {
        if (rate <= 0 || rate >= 1)
        {
            throw new ArgumentException("Discount rate must be between 0 and 1.");
        }

        Rate = rate;
    }
    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        return subtotal * Rate;
    }
}