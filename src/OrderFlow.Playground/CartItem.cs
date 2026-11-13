namespace OrderFlow.Playground;
class CartItem
{
    public Product Product { get; }
    public int Quantity { get; }

    public CartItem(Product product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        Product = product;
        Quantity = quantity;
    }

    public decimal LineTotal => Product.UnitPrice * Quantity;

}
