# Order-Flow

A hands-on .NET learning journey: building a production-oriented order management API with ASP.NET Core, step by step.

## Tech stack

- .NET 10, C# 14
- ASP.NET Core (coming soon)

## How to run

### Prerequisites

- .NET SDK 10.0.401 or a later 10.0 SDK (pinned in `global.json`)

### Run

From the repository root:

```bash
dotnet run --project src/OrderFlow.Playground
```

### Try it

- Enter a SKU from the catalog (e.g. `COF-001`, `TEA-001`). SKUs are case-insensitive.
- Enter a quantity. Stock is checked before the item is added to the cart.
- Press Enter on an empty SKU line to finish shopping.
- Use discount code `WELCOME10` for 10% off, or press Enter to skip.
- Test data: `CRO-001` is out of stock, `MUG-001` is not for sale.

### Example

A cart with two items and the welcome discount (free shipping from 1,000 TL):

```
SKU (Press Enter to finish): COF-001
Quantity: 40
Added: 40 x Filter Coffee
SKU (Press Enter to finish): TEA-001
Quantity: 2
Added: 2 x Green Tea
SKU (Press Enter to finish): 
===Your Cart===
1. Filter Coffee - 40 x 189,90 TL = 7.596,00 TL
2. Green Tea - 2 x 64,50 TL = 129,00 TL
Subtotal: 7.725,00 TL
Discount code (press Enter to skip): WELCOME10
Discount: -772,50 TL
Shipping Fee: 0,00 TL
VAT (%20): 1.390,50 TL
Total: 8.343,00 TL
```

> Number formatting follows your system language. The example above was produced on a Turkish (`tr-TR`) system; on an English system you will see `7,596.00` and `20%`.