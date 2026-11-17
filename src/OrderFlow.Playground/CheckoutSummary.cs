namespace OrderFlow.Playground;

record CheckoutSummary(decimal Subtotal, decimal Discount, decimal ShippingFee, decimal Vat)
{
    public decimal DiscountedSubtotal => Subtotal - Discount;
    public decimal Total => DiscountedSubtotal + ShippingFee + Vat;
}