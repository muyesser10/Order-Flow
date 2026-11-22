namespace OrderFlow.Playground;

class MinimumSubtotalFixedDiscount : FixedAmountDiscount
{
    public decimal MinimumSubtotal { get; }

    public MinimumSubtotalFixedDiscount(decimal amount, decimal minimumSubtotal) : base(amount)
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

        return Amount;
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