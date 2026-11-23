# API Contract (draft)

Draft from Lesson 3 (web fundamentals). It describes the API before the code exists and will become an OpenAPI specification in Lesson 5.

## Conventions

- Resources are plural, lowercase nouns: `/products`.
- The HTTP method is the verb. URLs contain no verbs.
- A path segment identifies one record: `/products/COF-001`.
- A query string filters a list: `/products?category=coffee`.
- Request and response bodies are JSON with camelCase property names.
- A list always returns `200`. An empty result is `[]`, not `404`.
- Authentication uses a bearer token (JWT) in the `Authorization` header (Lesson 7).

## Status codes

Checks run in this order: `401` → `403` → `404` → `400`.

| Code | Meaning | When |
|---|---|---|
| `200 OK` | Success | A record or a list was read, or a record was updated |
| `201 Created` | Success | A new record was created |
| `204 No Content` | Success | A record was deleted, nothing to return |
| `400 Bad Request` | Client error | The request body is invalid |
| `401 Unauthorized` | Client error | No valid token: the caller is unknown |
| `403 Forbidden` | Client error | The caller is known but not allowed |
| `404 Not Found` | Client error | The record named in the URL does not exist |
| `500 Internal Server Error` | Server error | An unexpected error on the server |

## Products

Reading is public. Creating, updating and deleting require the admin role.

| Action | Method and URL | Request body | Success | Errors |
|---|---|---|---|---|
| List products | `GET /products` | none | `200` | none |
| Filter by category | `GET /products?category=coffee` | none | `200` | none |
| Get one product | `GET /products/{sku}` | none | `200` | `404` |
| Create a product | `POST /products` | product fields | `201` | `401`, `403`, `400` |
| Update a product | `PATCH /products/{sku}` | changed fields only | `200` | `401`, `403`, `404`, `400` |
| Delete a product | `DELETE /products/{sku}` | none | `204` | `401`, `403`, `404` |

### Create a product

```http
POST /products
Content-Type: application/json
Authorization: Bearer <token>

{
  "sku": "COF-002",
  "name": "Cold Brew",
  "unitPrice": 159.90,
  "stockQuantity": 25,
  "isActive": true,
  "category": "Coffee"
}
```

Returns `400` when the SKU or name is empty, the unit price is zero or less, or the stock quantity is negative.

### Update the price

```http
PATCH /products/COF-001
Content-Type: application/json
Authorization: Bearer <token>

{ "unitPrice": 199.90 }
```