# BudgetWise API

A zero-based budgeting REST API inspired by [EveryDollar](https://www.everydollar.com/). Built with ASP.NET Core 8, Entity Framework Core, and JWT authentication.

---

## About

BudgetWise lets users create monthly budget plans, organize spending into categories and lines, record transactions, and manage sinking funds — all through a clean, versioned REST API.

The zero-based budgeting model means every dollar of income is assigned a job. Each month starts fresh with a plan, and transactions are recorded against budget lines to track planned vs. actual spending.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Authentication | ASP.NET Identity + JWT (access + refresh tokens) |
| Documentation | Swagger / OpenAPI 3.0 |
| Logging | Serilog |
| Testing | xUnit, Moq, FluentAssertions |

---

## Architecture

The solution follows a layered architecture with four projects:

```
BudgetWise.API            → Controllers, middleware, Program.cs
BudgetWise.Core           → Domain entities, DTOs, enums, models
BudgetWise.Infrastructure → AppDbContext, EF Core config, services
BudgetWise.Tests          → Unit tests
```

**Key decisions:**
- **Service layer** handles all business logic and ownership enforcement — users can never access another user's data
- **DTOs** decouple the API contract from the database schema
- **Fluent API** configuration for all EF Core relationships, decimal precision, indexes, and cascade rules
- **RFC 7807 Problem Details** for consistent error responses across all endpoints
- **JWT with refresh tokens** for stateless, secure authentication

---

## Data Model

```
AppUser (Identity)
  └── BudgetPlan (one per month/year)
        └── BudgetCategory
              └── BudgetLine
                    └── Transaction
  └── SinkingFund
```

- **BudgetPlan** — one plan per user per month/year, holds total income
- **BudgetCategory** — groups of budget lines (e.g. Housing, Food, Transportation)
- **BudgetLine** — individual budget items with planned amounts, supports recurring lines
- **Transaction** — actual money movement recorded against a budget line
- **SinkingFund** — named savings goals funded over multiple months (e.g. Christmas, Car Repair)

---

## API Endpoints

All endpoints require JWT authentication except `/api/v1/auth/register` and `/api/v1/auth/login`.

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | /api/v1/auth/register | Register a new user |
| POST | /api/v1/auth/login | Login and receive tokens |
| POST | /api/v1/auth/refresh | Refresh access token |
| POST | /api/v1/auth/logout | Invalidate refresh token |

### Budget Plans
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/v1/budget-plans | List all plans |
| POST | /api/v1/budget-plans | Create a new plan |
| GET | /api/v1/budget-plans/{id} | Get a specific plan |
| PUT | /api/v1/budget-plans/{id} | Update plan income |
| DELETE | /api/v1/budget-plans/{id} | Delete a plan |

### Budget Categories
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/v1/budget-plans/{planId}/categories | List categories |
| POST | /api/v1/budget-plans/{planId}/categories | Create a category |
| GET | /api/v1/budget-plans/{planId}/categories/{id} | Get a category |
| PUT | /api/v1/budget-plans/{planId}/categories/{id} | Update a category |
| DELETE | /api/v1/budget-plans/{planId}/categories/{id} | Delete a category |

### Budget Lines
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/v1/budget-plans/{planId}/categories/{catId}/lines | List lines |
| POST | /api/v1/budget-plans/{planId}/categories/{catId}/lines | Create a line |
| GET | /api/v1/budget-plans/{planId}/categories/{catId}/lines/{id} | Get a line |
| PUT | /api/v1/budget-plans/{planId}/categories/{catId}/lines/{id} | Update a line |
| DELETE | /api/v1/budget-plans/{planId}/categories/{catId}/lines/{id} | Delete a line |

### Transactions
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/v1/budget-plans/{planId}/categories/{catId}/lines/{lineId}/transactions | List transactions (paginated) |
| POST | /api/v1/budget-plans/{planId}/categories/{catId}/lines/{lineId}/transactions | Record a transaction |
| GET | .../transactions/{id} | Get a transaction |
| PUT | .../transactions/{id} | Update a transaction |
| DELETE | .../transactions/{id} | Delete a transaction |

### Sinking Funds
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/v1/sinking-funds | List all sinking funds |
| POST | /api/v1/sinking-funds | Create a sinking fund |
| GET | /api/v1/sinking-funds/{id} | Get a sinking fund |
| PUT | /api/v1/sinking-funds/{id} | Update a sinking fund |
| DELETE | /api/v1/sinking-funds/{id} | Delete a sinking fund |

---

## Running Locally

### Prerequisites
- .NET 8 SDK
- SQL Server (or SQL Server LocalDB)
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository:
```bash
git clone https://github.com/SethWatson91/BudgetWise.git
cd BudgetWise
```

2. Set up user secrets in `BudgetWise.API`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BudgetWiseDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-at-least-32-characters",
    "Issuer": "BudgetWiseAPI",
    "Audience": "BudgetWiseClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

3. Apply migrations:
```bash
dotnet ef database update --project BudgetWise.Infrastructure --startup-project BudgetWise.API
```

4. Run the API:
```bash
dotnet run --project BudgetWise.API
```

5. Open Swagger at `https://localhost:{port}/swagger`

---

## Testing

```bash
dotnet test
```

Unit tests cover service layer business logic including ownership enforcement, duplicate plan prevention, and CRUD operations.

---

## License

MIT
