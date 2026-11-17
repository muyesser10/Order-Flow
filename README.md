# Order-Flow

A hands-on .NET learning journey: building a production-oriented order management API with ASP.NET Core, step by step.

## Tech stack

- .NET 10, C# 14
- ASP.NET Core (coming soon)

## Features

- **Product catalog** with categories and stock. Products validate themselves: a product with an empty SKU, a non-positive price or negative stock cannot be created.
- **Supplier import:** products are loaded from `products.csv` at startup. Malformed rows are skipped and reported; a missing file does not stop the program.
- **Catalog report:** out-of-stock products, products sorted by price, products per category and total inventory value.
- **Discount codes:**

  | Code | Rule |
  |---|---|
  | `WELCOME10` | 10% off |
  | `FLAT50` | 50 TL off (never more than the subtotal) |
  | `BIG20` | 20% off for orders of 1,000 TL or more |

- **Checkout:** subtotal, discount, shipping (free from 1,000 TL) and 20% VAT.

## How to run

### Prerequisites

- .NET SDK 10.0.401 or a later 10.0 SDK (pinned in `global.json`)

### Run

From the repository root:

```bash
dotnet run --project src/OrderFlow.Playground
```

### Try it

- At startup the supplier file is imported. Two of its rows are invalid on purpose, so you will see two `Skipped` lines.
- Enter a SKU from the catalog (e.g. `COF-001`, `TEA-001`). SKUs are case-insensitive.
- Enter a quantity. Stock is checked before the item is added to the cart.
- Press Enter on an empty SKU line to finish shopping.
- Enter a discount code (`WELCOME10`, `FLAT50`, `BIG20`) or press Enter to skip. If a code does not apply to your cart, you will see why.
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

## Project structure

```
src/OrderFlow.Playground/
├── Program.cs                  # console flow: import, report, cart, checkout
├── products.csv                # sample supplier file (two invalid rows on purpose)
├── Product.cs
├── ProductCategory.cs
├── CartItem.cs
├── CheckoutSummary.cs
├── IDiscount.cs                # discount contract
├── DiscountBase.cs             # shared rules: cap at subtotal, rounding
├── PercentageDiscount.cs
├── FixedAmountDiscount.cs
└── MinimumSubtotalDiscount.cs
```

## Learning progress

| Lesson | Topic | Status |
|---|---|---|
| 0 | Setup and AI-assisted workflow | Done |
| 1 | C# fundamentals | Done |
| 2 | OOP and modern C# | Done |
| 3 | Web fundamentals | Next |

Concepts practiced in Lesson 2: encapsulation, interfaces, inheritance, abstract classes and the template method pattern, enums, records, generics, delegates and lambdas, LINQ, exceptions, async/await.