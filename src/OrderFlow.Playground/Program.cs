string productName = "Filter Coffee";
string sku = "COF-001";

decimal unitPrice = 189.90m;
int stockQuantity = 42;

bool isActive = true;

const decimal VatRate = 0.20m;

Console.WriteLine($"Product Name: {productName} ({sku})");
Console.WriteLine($"Price: {unitPrice}");
Console.WriteLine($"Stock: {stockQuantity}");