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
    Console.WriteLine($"Quantity received: {quantity}");
}
else
{
    Console.WriteLine("Please enter a valid number.");
}