# Dapper.Common

> A lightweight, opinionated framework for using **Dapper** with **connection lifecycle**, **transaction management**, and **EF Core–style configuration**.

`Dapper.Common` provides a structured way to work with Dapper while keeping full control over SQL, performance, and database-specific features — without introducing an ORM.

---

## ✨ Key Features

- ✅ Explicit **connection lifecycle management**
- ✅ Built-in **Unit of Work** and **transaction handling**
- ✅ EF Core–style configuration (`UseMySql`, `UseSqlServer`, etc.)
- ✅ No hidden magic — SQL stays explicit
- ✅ Designed for **Clean Architecture**
- ✅ Zero dependency on EF Core

---

## 📦 Installation

```bash
dotnet add package DapperContext
```

## 🚀 Getting Started

### 1️⃣ Configure services

```csharp
services.AddDapperContext(builder =>
{
    builder.UseMySql("Server=localhost;Database=app;Uid=root;Pwd=123;");
});
```

# Or using a connection string from configuration:

```csharp
services.AddDapperContext(builder =>
{
    builder.UseMySqlFromConnectionStringName("Default");
});
```

### 2️⃣ Inject DapperContext into repositories

```csharp
public sealed class UserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        const string sql = """
            SELECT id, name, email
            FROM users
            WHERE id = @id
        """;

        return _context.QueryFirstOrDefaultAsync<User>(
            sql,
            new { id },
            ct);
    }
}
```
#### 📌 This is the recommended way to interact with the database.

### 🔁 Transactions & Unit of Work

DapperContext automatically participates in an active transaction if one exists.

To explicitly control transactions, inject IUnitOfWork:

```csharp
public sealed class CreateUserHandler
{
    private readonly DapperContext _context;
    private readonly IUnitOfWork _uow;

    public CreateUserHandler(DapperContext context, IUnitOfWork uow)
    {
        _context = context;
        _uow = uow;
    }

    public async Task HandleAsync(CreateUser command, CancellationToken ct)
    {
        await _uow.BeginTransactionAsync(ct);

        try
        {
            await _context.ExecuteAsync(
                "INSERT INTO users (id, name) VALUES (@id, @name)",
                new { command.Id, command.Name },
                ct);

            await _uow.CommitAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}
```
✔️ All commands executed through DapperContext automatically use the current transaction.

## 🧱 Architecture Overview
```pgsql
┌──────────────────────┐
│     Application      │
│  (Handlers, UseCases)│
└──────────▲───────────┘
           │
┌──────────┴───────────┐
│    DapperContext     │  ← Recommended API
│ (Queries & Commands) │
└──────────▲───────────┘
           │
┌──────────┴───────────┐
│      DbSession       │  ← Connection & transaction lifecycle
│ (IDbSession + UoW)   │
└──────────▲───────────┘
           │
┌──────────┴───────────┐
│     ADO.NET / DB     │
└──────────────────────┘
```

## ⚙️ Advanced Usage: IDbSession

For advanced scenarios, the framework exposes IDbSession.

### When should you use it?

- Bulk operations
- Database-specific features
- Integration with third-party libraries
- Low-level ADO.NET access

```csharp
public sealed class RawSqlService
{
    private readonly IDbSession _session;

    public RawSqlService(IDbSession session)
    {
        _session = session;
    }

    public async Task ExecuteAsync(CancellationToken ct)
    {
        await _session.OpenAsync(ct);

        using var cmd = _session.Connection.CreateCommand();
        cmd.CommandText = "SET some_db_specific_flag = 1";
        cmd.Transaction = _session.Transaction;

        await cmd.ExecuteNonQueryAsync(ct);
    }
}
```
⚠️ Note:
Using IDbSession bypasses some safeguards provided by DapperContext.
Prefer DapperContext unless you know exactly what you’re doing.

## 🧩 Supported Providers

- MySQL
- Oracle
- SQLite

Providers are configured via extension methods:
```csharp
builder.UseMySql(...)
builder.UseOracle(...)
builder.UseSqlite(...)
```
Only one provider can be configured per application.

## 🛠 Design Philosophy

- Explicit over implicit
- Control over convenience
- Opinionated, but extensible
- Framework-level guarantees, not magic

This framework does not:
- Track entities
- Generate SQL
- Hide database behavior

## 🆚 Why not just Dapper?

You absolutely can.

DapperContext exists for teams that want:
- Consistent transaction handling
- Centralized connection lifecycle
- Cleaner repositories
- EF Core–like ergonomics without EF Core

## 🧪 Thread Safety & Lifetime

- DbSession, DapperContext, and UnitOfWork are Scoped
- One connection per scope
- One transaction per scope

## 📄 License

MIT

## ⭐ Final Notes

This framework is intentionally small.

If you ever feel the need to fight it —
you can always drop down to IDbSession.

That’s by design.