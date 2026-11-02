const decimal VatRate = 0.20m;
const string WelcomeCode = "WELCOME10";
const decimal WelcomeDiscountRate = 0.10m;

var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
{
    ["COF-001"] = new Product { Sku = "COF-001", Name = "Filter Coffee", UnitPrice = 189.90m, StockQuantity = 42, IsActive = true },
    ["TEA-001"] = new Product { Sku = "TEA-001", Name = "Green Tea", UnitPrice = 64.50m, StockQuantity = 10, IsActive = true },
    ["CRO-001"] = new Product { Sku = "CRO-001", Name = "Croissant", UnitPrice = 45.00m, StockQuantity = 0, IsActive = true },
    ["MUG-001"] = new Product { Sku = "MUG-001", Name = "Ceramic Mug", UnitPrice = 249.00m, StockQuantity = 5, IsActive = false }
};

Console.WriteLine("====Catalog====");

foreach (Product product in catalog.Values)
{
    Console.WriteLine($"SKU: {product.Sku}, Name: {product.Name}, Price: {product.UnitPrice:N2} TL, Stock: {product.StockQuantity}");
}

var cart = new List<CartItem>();

while (true)
{
    Console.Write("SKU (Press Enter to finish): ");
    string? skuInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(skuInput))
    {
        break;
    }

    if (!catalog.TryGetValue(skuInput.Trim(), out Product? product))
    {
        Console.WriteLine("Product not found.");
        continue;
    }

    if (!product.IsActive)
    {
        Console.WriteLine("This product is not available.");
        continue;
    }

    Console.Write("Quantity: ");

    if (!int.TryParse(Console.ReadLine(), out int quantity))
    {
        Console.WriteLine("Please enter a valid number.");
        continue;
    }

    if (quantity <= 0)
    {
        Console.WriteLine("Quantity must be greater than zero.");
        continue;
    }

    if (quantity > product.StockQuantity)
    {
        Console.WriteLine($"Insufficient stock. Available quantity: {product.StockQuantity}");
        continue;
    }

    cart.Add(new CartItem { Product = product, Quantity = quantity });
    product.StockQuantity -= quantity;
    Console.WriteLine($"Added: {quantity} x {product.Name}");

}

if (cart.Count == 0)
{
    Console.WriteLine("Your cart is empty.");
    return;
}

Console.WriteLine("===Your Cart===");
decimal subtotal = 0m;

for (int i = 0; i < cart.Count; i++)
{
    CartItem cartItem = cart[i];
    decimal lineTotal = cartItem.Product.UnitPrice * cartItem.Quantity;
    subtotal += lineTotal;
    Console.WriteLine($"{i + 1}. {cartItem.Product.Name} - {cartItem.Quantity} x {cartItem.Product.UnitPrice:N2} TL = {lineTotal:N2} TL");
}

Console.WriteLine($"Subtotal: {subtotal:N2} TL");

Console.Write("Discount code (press Enter to skip): ");
string? discountInput = Console.ReadLine();

decimal discountRate;
if (string.IsNullOrWhiteSpace(discountInput))
{
    discountRate = 0m;
}
else if (string.Equals(discountInput.Trim(), WelcomeCode, StringComparison.OrdinalIgnoreCase))
{
    discountRate = WelcomeDiscountRate;
}
else
{
    Console.WriteLine("Invalid discount code. No discount applied.");
    discountRate = 0m;
}

decimal discountAmount = Math.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
decimal discountedSubtotal = subtotal - discountAmount;

decimal shippingFee = discountedSubtotal switch
{
    >= 1000 => 0m,
    >= 500 => 29.90m,
    _ => 49.90m
};

decimal vatAmount = Math.Round(discountedSubtotal * VatRate, 2, MidpointRounding.AwayFromZero);
decimal total = discountedSubtotal + shippingFee + vatAmount;

if (discountAmount > 0)
{
    Console.WriteLine($"Discount: -{discountAmount:N2} TL");
}
Console.WriteLine($"Shipping Fee: {shippingFee:N2} TL");
Console.WriteLine($"VAT ({VatRate:P0}): {vatAmount:N2} TL");
Console.WriteLine($"Total: {total:N2} TL");

class Product
{
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
}

class CartItem
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}