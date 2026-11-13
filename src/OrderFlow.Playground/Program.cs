const decimal VatRate = 0.20m;

var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
{
    ["COF-001"] = new Product(sku: "COF-001", name: "Filter Coffee", unitPrice: 189.90m, stockQuantity: 42, isActive: true, category: ProductCategory.Coffee),
    ["TEA-001"] = new Product(sku: "TEA-001", name: "Green Tea", unitPrice: 64.50m, stockQuantity: 10, isActive: true, category: ProductCategory.Tea),
    ["CRO-001"] = new Product(sku: "CRO-001", name: "Croissant", unitPrice: 45.00m, stockQuantity: 0, isActive: true, category: ProductCategory.Bakery),
    ["MUG-001"] = new Product(sku: "MUG-001", name: "Ceramic Mug", unitPrice: 249.00m, stockQuantity: 5, isActive: false, category: ProductCategory.Merchandise),
    ["ESP-001"] = new Product(sku: "ESP-001", name: "Espresso Beans", unitPrice: 279.00m, stockQuantity: 15, isActive: true, category: ProductCategory.Coffee)
};

var discounts = new Dictionary<string, IDiscount>(StringComparer.OrdinalIgnoreCase)
{
    ["WELCOME10"] = new PercentageDiscount(0.10m),
    ["FLAT50"] = new FixedAmountDiscount(50m),
    ["BIG20"] = new MinimumSubtotalDiscount(0.20m, 1000m)
};

Console.WriteLine("====Catalog====");

foreach (Product product in catalog.Values.OrderByDescending(p => p.UnitPrice))
{
    Console.WriteLine(product.GetDisplayText());
}

List<Product> outOfStock = catalog.Values.Where(p => p.StockQuantity == 0).ToList();

if (outOfStock.Count > 0)
{
    Console.WriteLine( "Out of stock: ");
    foreach (Product p in outOfStock)
    {
        Console.WriteLine($"- {p.Name} ");
    }
}

Console.WriteLine("Products per category: ");
foreach ( var group in catalog.Values.GroupBy(p => p.Category))
{
    Console.WriteLine($"- {group.Key}: {group.Count()}");
}

decimal inventoryValue = catalog.Values.Sum(p => p.UnitPrice * p.StockQuantity);
Console.WriteLine($"Inventory value: {inventoryValue:N2} TL");

bool hasOutOfStock = catalog.Values.Any(p => p.StockQuantity == 0);
Console.WriteLine($"Any out of stock: {hasOutOfStock}");

var cart = new List<CartItem>();

while (true)
{
    Console.Write("SKU (Press Enter to finish): ");
    string? skuInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(skuInput))
    {
        break;
    }

    Product? product = FindByCode(catalog, skuInput);
    if (product == null)
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

    if (!product.TryDecreaseStock(quantity))
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


for (int i = 0; i < cart.Count; i++)
{
    CartItem cartItem = cart[i];
    decimal lineTotal = cartItem.LineTotal;
    Console.WriteLine($"{i + 1}. {cartItem.Product.Name} - {cartItem.Quantity} x {cartItem.Product.UnitPrice:N2} TL = {lineTotal:N2} TL");
}

decimal subtotal = cart.Sum(item => item.LineTotal);

Console.WriteLine($"Subtotal: {subtotal:N2} TL");

Console.Write("Discount code (press Enter to skip): ");
string? discountInput = Console.ReadLine();

decimal discountAmount = 0m;

if (!string.IsNullOrWhiteSpace(discountInput))
{
    IDiscount? discount = FindByCode(discounts, discountInput);
    if (discount != null)
    {
        discountAmount = discount.CalculateDiscount(subtotal);
    }
    else
    {
        Console.WriteLine("Invalid discount code. No discount applied.");
    }
}
CheckoutSummary summary = CalculateCheckout(subtotal, discountAmount);

if (discountAmount > 0)
{
    Console.WriteLine($"Discount: -{summary.Discount:N2} TL");
}
PrintMoneyLine("Shipping Fee", summary.ShippingFee);
Console.WriteLine($"VAT ({VatRate:P0}): {summary.Vat:N2} TL");
PrintMoneyLine("Total", summary.Total);


decimal CalculateVat(decimal amount)
{
    return Math.Round(amount * VatRate, 2, MidpointRounding.AwayFromZero);
}

T? FindByCode<T>(Dictionary<string, T> source, string input) where T : class
{
    if (source.TryGetValue(input.Trim(), out T? value))
    {
        return value;
    }

    return null;
}


void PrintMoneyLine(string label, decimal amount)
{
    Console.WriteLine($"{label}: {amount:N2} TL");
}

CheckoutSummary CalculateCheckout(decimal subtotal, decimal discountAmount)
{
    decimal discountedSubtotal = subtotal - discountAmount;

    decimal shippingFee = discountedSubtotal switch
    {
        >= 1000 => 0m,
        >= 500 => 29.90m,
        _ => 49.90m
    };

    decimal vatAmount = CalculateVat(discountedSubtotal);

    return new CheckoutSummary(subtotal, discountAmount, shippingFee, vatAmount);
}

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

abstract class DiscountBase : IDiscount
{
    public decimal CalculateDiscount(decimal subtotal)
    {
        decimal rawDiscount = CalculateRawDiscount(subtotal);
        decimal cappedDiscount = Math.Min(rawDiscount, subtotal);
        return Math.Round(cappedDiscount, 2, MidpointRounding.AwayFromZero);
    }
    protected abstract decimal CalculateRawDiscount(decimal subtotal);
}

class PercentageDiscount : DiscountBase
{
    public decimal Rate { get; }

    public PercentageDiscount(decimal rate)
    {
        if (rate <= 0 || rate >= 1)
        {
            throw new ArgumentException("Discount rate must be between 0 and 1.");
        }

        Rate = rate;
    }
    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        return subtotal * Rate;
    }
}

class MinimumSubtotalDiscount : PercentageDiscount
{
    public decimal MinimumSubtotal { get; }

    public MinimumSubtotalDiscount(decimal rate, decimal minimumSubtotal) : base(rate)
    {
        if (minimumSubtotal <= 0)
        {
            throw new ArgumentException("Minimum subtotal must be greater than zero.");
        }

        MinimumSubtotal = minimumSubtotal;
    }

    protected override decimal CalculateRawDiscount(decimal subtotal)
    {
        if (subtotal < MinimumSubtotal)
        {
            return 0m;
        }

        return base.CalculateRawDiscount(subtotal);
    }

}

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

enum ProductCategory
{
    Coffee,
    Tea,
    Bakery,
    Merchandise
}
record CheckoutSummary(decimal Subtotal, decimal Discount, decimal ShippingFee, decimal Vat)
{
    public decimal DiscountedSubtotal => Subtotal - Discount;
    public decimal Total => DiscountedSubtotal + ShippingFee + Vat;
}