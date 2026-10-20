string productName = "Filter Coffee";
string sku = "COF-001";
bool isActive = true;

decimal unitPrice = 189.90m;
int stockQuantity = 42;

const decimal VatRate = 0.20m;

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
    { Console.WriteLine("This product is not available.");}

    else if (quantity > stockQuantity)
    { Console.WriteLine($"Insufficient stock. Available: {stockQuantity}"); }

    else
    {
        decimal subtotal = unitPrice * quantity;
        decimal vatAmount = Math.Round(subtotal * VatRate, 2);
        decimal total = subtotal + vatAmount;
        Console.WriteLine($"Subtotal: {subtotal}");
        Console.WriteLine($"VAT {VatRate:P0}: {vatAmount}");
        Console.WriteLine($"Total: {total:N2}");
    }
}
else
{
    Console.WriteLine("Please enter a valid number.");
}