abstract class DiscountBase : IDiscount
{
    public decimal CalculateDiscount(decimal subtotal)
    {
        decimal rawDiscount = CalculateRawDiscount(subtotal);
        decimal cappedDiscount = Math.Min(rawDiscount, subtotal);
        return Math.Round(cappedDiscount, 2, MidpointRounding.AwayFromZero);
    }
    protected abstract decimal CalculateRawDiscount(decimal subtotal);

    public virtual string? GetIneligibilityReason(decimal subtotal)
    {
        return null;
    }
}
