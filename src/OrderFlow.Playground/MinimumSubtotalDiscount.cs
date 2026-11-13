class MinimumSubtotalDiscount : PercentageDiscount
{
    public decimal MinimumSubtotal { get; }

    public MinimumSubtotalDiscount(decimal rate, decimal minimumSubtotal) : base(rate)
    {
        if (minimumSubtotal <= 0)
        {
            throw new ArgumentException("Minimum subtotal must be greater than zero.");
        }

        MinimumSubtotal = minimumSubtotal;
    }

    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        if (subtotal < MinimumSubtotal)
        {
            return 0m;
        }

        return base.CalculateRawDiscount(subtotal);
    }

    public override string? GetIneligibilityReason(decimal subtotal)
    {
        if (subtotal < MinimumSubtotal)
        {
            return $"This code is valid for orders of {MinimumSubtotal:N2} TL or more.";
        }

        return null;
    }

}