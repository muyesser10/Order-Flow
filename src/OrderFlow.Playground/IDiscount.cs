interface IDiscount
{
    decimal CalculateDiscount(decimal subtotal);
    string? GetIneligibilityReason(decimal subtotal);
}