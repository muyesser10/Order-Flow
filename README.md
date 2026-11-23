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
  | `CAP25` | 25% off, up to 100 TL |
  | `SAVE30` | 30 TL off for orders of 300 TL or more |

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
- Enter a discount code (`WELCOME10`, `FLAT50`, `BIG20`, `CAP25`, `SAVE30`) or press Enter to skip. If a code does not apply to your cart, you will see why.
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
docs/
└── api-contract.md             # draft REST API contract for products
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
├── MinimumSubtotalDiscount.cs
├── CappedPercentageDiscount.cs
└── MinimumSubtotalFixedDiscount.cs
```
## How AI is used in this project

This project is also practice in working with AI the way many teams do today. Two tools are involved, each with clear rules.

**Claude as a tutor (Lessons 0-3)**

- Explains each topic first: the theory, then how it is used in real business projects.
- Plans how the topic fits into Order-Flow and asks me to write the logic as pseudocode before any C#.
- Gives hints instead of answers when I get stuck, and goes through corrections line by line.
- Ends every session with a quiz. The questions I miss come back at the start of the next session.

**Claude Code in the repository (manual mode)**

- Its rules live in `CLAUDE.md`: in Lessons 0-3 it may read the code, explain it and suggest improvements, but it may not create or edit files.
- I decide which suggestions to apply. I type every code change myself and do not commit code I cannot explain.
- English documentation, such as the API contract, is drafted together with the tutor from my own design.

**What changes from Lesson 4**

- Claude Code may generate repetitive scaffolding. I write the core logic myself and review every diff line by line before committing.

## Learning progress

| Lesson | Topic | Status |
|---|---|---|
| 0 | Setup and AI-assisted workflow | Done |
| 1 | C# fundamentals | Done |
| 2 | OOP and modern C# | Done |
| 3 | Web fundamentals | Done |
| 4 | ASP.NET Core fundamentals | Next |

Concepts practiced in Lesson 2: encapsulation, interfaces, inheritance, abstract classes and the template method pattern, enums, records, generics, delegates and lambdas, LINQ, exceptions, async/await.Concepts practiced in Lesson 3: HTTP requests and responses, methods and status codes, REST resource design, JSON, cookies, sessions and JWT, calling an API with `fetch`, and designing an API contract before writing code.