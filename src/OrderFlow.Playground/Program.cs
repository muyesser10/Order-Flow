string productName = "Filter Coffee";
string sku = "COF-001";
bool isActive = true;

decimal unitPrice = 189.90m;
int stockQuantity = 42;

const decimal VatRate = 0.20m;
const string WelcomeCode = "WELCOME10";
const decimal WelcomeDiscountRate = 0.10m;

Console.WriteLine($"Product Name: {productName} ({sku})");
Console.WriteLine($"Price: {unitPrice}");
Console.WriteLine($"Stock: {stockQuantity}");

Console.Write("Quantity: ");
string? input = Console.ReadLine();
if (int.TryParse(input, out int quantity))

{
    if (quantity <= 0)
    { Console.WriteLine("Quantity must be greater than zero."); }

    else if (!isActive)
    { Console.WriteLine("This product is not available."); }

    else if (quantity > stockQuantity)
    { Console.WriteLine($"Insufficient stock. Available: {stockQuantity}"); }

    else
    {
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
            Console.WriteLine("Invalid discount code.");
            discountRate = 0m;
        }

        decimal subtotal = unitPrice * quantity;
        decimal discountAmount = Math.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        decimal discountedSubtotal = subtotal - discountAmount;
        decimal vatAmount = Math.Round(discountedSubtotal * VatRate, 2, MidpointRounding.AwayFromZero);
        decimal total = discountedSubtotal + vatAmount;

        Console.WriteLine($"Subtotal: {subtotal:N2} TL");

        if (discountAmount > 0)
        {
            Console.WriteLine($"Discount: -{discountAmount:N2} TL");
        }
        Console.WriteLine($"VAT ({VatRate:P0}): {vatAmount:N2} TL");
        Console.WriteLine($"Total: {total:N2} TL");


    }
}
else
{
    Console.WriteLine("Please enter a valid number.");
}