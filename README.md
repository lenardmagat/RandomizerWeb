# RandomizerWeb

A REST API for splitting a list of people into randomized, evenly-sized groups — built with ASP.NET Core and PostgreSQL.

Create a group (e.g. "Team Standup", "Secret Santa", "Study Groups"), give it a list of member names and how many sub-groups you want, and the API shuffles everyone and deals them out round-robin style. Groups can be created anonymously or while logged in, and logged-in users can save, revisit, and manage members across multiple groups.

## Features

- **Randomized group splitting** — members are shuffled (`Random.Shared.Shuffle`) and dealt into N sub-groups in round-robin order.
- **Anonymous or authenticated group creation** — no account needed to generate a quick split, but registered users get persistence and a group history.
- **JWT authentication** — registration/login with BCrypt-hashed passwords and short-lived (2-hour) JWTs.
- **Shareable group links** — group IDs are obfuscated with Hashids before being exposed, so raw database IDs are never leaked in URLs.
- **Member roster management** — authenticated users can maintain a member list, with names implicitly marked active/inactive as they're added or removed from future groups.
- **Structured error handling** — a global exception filter and a `Result`/`Result<T>` pattern for consistent success/failure responses.
- **Swagger / OpenAPI UI** — interactive API docs available in development mode.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 10) |
| Database | PostgreSQL via Npgsql + EF Core |
| Auth | JWT Bearer tokens, BCrypt.Net for password hashing |
| ID obfuscation | Hashids.net |
| Logging | Serilog (console sink) |
| API docs | Swashbuckle / Scalar (OpenAPI) |
| Config | DotNetEnv (`.env` file support) |

## Project Structure

```
RandomizerWeb/
├── Application/
│   ├── DTOs/            # Request/response contracts
│   ├── Interfaces/      # Service contracts
│   └── Services/        # Business logic (account + group services)
├── Controllers/         # API endpoints (AccountController, GroupController)
├── Infastructure/
│   ├── DataBase/        # EF Core DbContext and DB manager
│   ├── Middleware/      # DI wiring, app configuration, exception middleware
│   ├── Repositories/    # Data access layer
│   └── Security/        # JWT/hashing/Hashids implementation, error handling
├── Migrations/          # EF Core migrations
├── Models/               # EF Core entities (User, Group, Member, GroupMember)
└── Program.cs            # App startup/bootstrap
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A PostgreSQL instance

### Configuration

The app reads its configuration from environment variables (or a `.env` file, via DotNetEnv). At minimum you'll need:

```env
DataBaseConnection=Host=localhost;Database=randomizerweb;Username=postgres;Password=yourpassword
JWT_KEY=your-secret-signing-key
JWT_ISSUER=PracticeWeb
JWT_AUDIENCE=PracticeWebUsers
```

### Run locally

```bash
git clone https://github.com/lenardmagat/RandomizerWeb.git
cd RandomizerWeb
dotnet restore
dotnet run
```

On startup, the app automatically applies pending EF Core migrations against the configured database.

In development mode, the Swagger UI is available at the app root (`/`).

## API Overview

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `POST` | `/API/Account/Create` | — | Register a new account |
| `POST` | `/API/Account/Login` | — | Log in and receive a JWT |
| `PATCH` | `/API/Account/Update-Profile` | ✅ | Update account/password |
| `POST` | `/API/Group/Create` | Optional | Create a group and randomly split members into N sub-groups |
| `GET` | `/API/Group/{GroupId}` | — | Fetch a group's split by its hashed ID |
| `GET` | `/API/Group/GroupData` | ✅ | List all groups owned by the current user |
| `POST` | `/API/Group/AddUpdate` | ✅ | Add or update a user's saved member roster |

## Status

This is a personal/practice project and a work in progress — expect incomplete validation, missing tests, and evolving conventions as features are added.

## License

No license specified yet — all rights reserved by default until one is added.
