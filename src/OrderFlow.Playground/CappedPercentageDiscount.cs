namespace OrderFlow.Playground;

class CappedPercentageDiscount : PercentageDiscount
{

    public decimal MaxDiscountAmount { get; }

    public CappedPercentageDiscount(decimal rate, decimal maxDiscountAmount) : base(rate)
    {
        if (maxDiscountAmount <= 0)
        {
            throw new ArgumentException("Maximum discount amount must be greater than zero.");
        }
        MaxDiscountAmount = maxDiscountAmount;
    }

    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        decimal rawDiscount = base.CalculateRawDiscount(subtotal);
        return Math.Min(rawDiscount, MaxDiscountAmount);

    }

}