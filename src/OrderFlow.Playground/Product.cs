namespace OrderFlow.Playground;
class Product
{
    public string Sku { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; }
    public ProductCategory Category { get; }

    public Product(string sku, string name, decimal unitPrice, int stockQuantity, bool isActive, ProductCategory category)
    {
        if (stockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero.");
        }

        Sku = sku;
        Name = name;
        UnitPrice = unitPrice;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        Category = category;
    }

    public string GetDisplayText()
    {
        return $"SKU: {Sku}, Name: {Name}, Price: {UnitPrice:N2} TL, Stock: {StockQuantity}, Category: {Category}";
    }

    public bool TryDecreaseStock(int quantity)
    {
        if (quantity <= 0 || quantity > StockQuantity)
        {
            return false;
        }

        StockQuantity -= quantity;
        return true;
    }

}