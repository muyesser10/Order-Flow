class FixedAmountDiscount : DiscountBase
{
    public decimal Amount { get; }

    public FixedAmountDiscount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Discount amount must be greater than zero.");
        }
        Amount = amount;
    }

    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        return Amount;
    }
}