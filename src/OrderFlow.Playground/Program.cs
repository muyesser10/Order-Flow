const decimal VatRate = 0.20m;

var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
{
    ["COF-001"] = new Product(sku: "COF-001", name: "Filter Coffee", unitPrice: 189.90m, stockQuantity: 42, isActive: true),
    ["TEA-001"] = new Product(sku: "TEA-001", name: "Green Tea", unitPrice: 64.50m, stockQuantity: 10, isActive: true),
    ["CRO-001"] = new Product(sku: "CRO-001", name: "Croissant", unitPrice: 45.00m, stockQuantity: 0, isActive: true),
    ["MUG-001"] = new Product(sku: "MUG-001", name: "Ceramic Mug", unitPrice: 249.00m, stockQuantity: 5, isActive: false)
};

var discounts = new Dictionary<string, IDiscount>(StringComparer.OrdinalIgnoreCase)
{
    ["WELCOME10"] = new PercentageDiscount(0.10m),
    ["FLAT50"] = new FixedAmountDiscount(50m)
};

Console.WriteLine("====Catalog====");

foreach (Product product in catalog.Values)
{
    Console.WriteLine(product.GetDisplayText());
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

    if(!product.TryDecreaseStock(quantity))
    {
        Console.WriteLine($"Insufficient stock. Available quantity: {product.StockQuantity}");
        continue;
    }

    cart.Add(new CartItem(product, quantity));
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
    decimal lineTotal = cartItem.LineTotal;
    subtotal += lineTotal;
    Console.WriteLine($"{i + 1}. {cartItem.Product.Name} - {cartItem.Quantity} x {cartItem.Product.UnitPrice:N2} TL = {lineTotal:N2} TL");
}

Console.WriteLine($"Subtotal: {subtotal:N2} TL");

Console.Write("Discount code (press Enter to skip): ");
string? discountInput = Console.ReadLine();

decimal discountAmount = 0m;

if (!string.IsNullOrWhiteSpace(discountInput))
{
    if (discounts.TryGetValue(discountInput.Trim(), out IDiscount? discount))
    {
        discountAmount = discount.CalculateDiscount(subtotal);
    }
    else
    {
        Console.WriteLine("Invalid discount code. No discount applied.");
    }
}

decimal discountedSubtotal = subtotal - discountAmount;

decimal shippingFee = discountedSubtotal switch
{
    >= 1000 => 0m,
    >= 500 => 29.90m,
    _ => 49.90m
};

decimal vatAmount = CalculateVat(discountedSubtotal);
decimal total = discountedSubtotal + shippingFee + vatAmount;

if (discountAmount > 0)
{
    Console.WriteLine($"Discount: -{discountAmount:N2} TL");
}
PrintMoneyLine("Shipping Fee", shippingFee);
Console.WriteLine($"VAT ({VatRate:P0}): {vatAmount:N2} TL");
PrintMoneyLine("Total", total);


decimal CalculateVat(decimal amount)
{
    return Math.Round(amount * VatRate, 2, MidpointRounding.AwayFromZero);
}


void PrintMoneyLine(string label, decimal amount)
{
    Console.WriteLine($"{label}: {amount:N2} TL");
}

class Product
{
    public string Sku { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; }

    public Product(string sku, string name, decimal unitPrice, int stockQuantity, bool isActive)
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
}

    public string GetDisplayText()
    {
        return $"SKU: {Sku}, Name: {Name}, Price: {UnitPrice:N2} TL, Stock: {StockQuantity}";
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

interface IDiscount
{
    decimal CalculateDiscount(decimal subtotal);
}

class PercentageDiscount: IDiscount
{
    public decimal Rate { get; }

    public PercentageDiscount(decimal rate)
    {
        if (rate < 0 || rate > 1)
        {
            throw new ArgumentException("Discount rate must be between 0 and 1.");
        }

        Rate = rate;
    }
    public decimal CalculateDiscount(decimal subtotal)
    {
        return Math.Round(subtotal * Rate, 2, MidpointRounding.AwayFromZero);
    }
}

class FixedAmountDiscount: IDiscount
{
    public decimal Amount {get; }

    public FixedAmountDiscount(decimal amount)
    { 
        if (amount <= 0)
        {
        throw new ArgumentException("Discount amount must be greater than zero.");
        }
        Amount = amount;
    }

    public decimal CalculateDiscount(decimal subtotal)
    {
        return Math.Min(Amount, subtotal);
    }

}