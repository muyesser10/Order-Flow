var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
{
   ["COF-001"] = new Product { Sku = "COF-001", Name = "Filter Coffee", UnitPrice = 189.90m, StockQuantity = 42, IsActive = true },
    ["TEA-001"] = new Product { Sku = "TEA-001", Name = "Green Tea", UnitPrice = 64.50m, StockQuantity = 10, IsActive = true },
    ["CRO-001"] = new Product { Sku = "CRO-001", Name = "Croissant", UnitPrice = 45.00m, StockQuantity = 0, IsActive = true },
    ["MUG-001"] = new Product { Sku = "MUG-001", Name = "Ceramic Mug", UnitPrice = 249.00m, StockQuantity = 5, IsActive = false }
};

Console.WriteLine("====Catalog====");

foreach(Product product in catalog.Values)
{
    Console.WriteLine($"SKU: {product.Sku}, Name: {product.Name}, Price: {product.UnitPrice:N2} TL, Stock: {product.StockQuantity}");
}

var cart = new List<CartItem>();

while(true)
{
    Console.Write("SKU (Press Enter to finish): ");
    string? skuInput = Console.ReadLine();

    if(string.IsNullOrWhiteSpace(skuInput))
    {
        break;
    }

    if(!catalog.TryGetValue(skuInput.Trim(), out Product? product))
    {
        Console.WriteLine("Product not found.");
        continue;
    }

    if (!product.IsActive)
    {
        Console.WriteLine("This product is not available.");
        continue;
    }

    Console.WriteLine($"Found: {product.Name}");
}

Console.WriteLine($"Items in cart: {cart.Count}");

class Product
{
    public string Sku {get; set;} ="";
    public string Name {get; set;} ="";
    public decimal UnitPrice {get;set;}
    public int StockQuantity {get;set;}
    public bool IsActive {get;set;}
}

class CartItem
{
    public Product Product {get;set;} = null!;
    public int Quantity {get;set;}
}