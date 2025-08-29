TestCustomerWinForms (.NET 8 Windows) — CRUD for dbo.tblTestCustomer

Prereqs
- Visual Studio 2022 17.8+ with "Desktop development with .NET"
- SQL Server Express instance DESKTOP-DEBD6R2\SQLEXPRESS
- Database TestGPT
- Optional: run schema.sql to create dbo.tblTestCustomer

Connection
- appsettings.json contains the connection string under "ConnectionStrings:TestGPT".
- Adjust to your machine if needed.

Build & Run
1. Open TestCustomerWinForms.Net8.sln
2. Restore packages (automatic): Microsoft.Data.SqlClient, Microsoft.Extensions.Configuration.Json
3. Press F5
4. Use buttons: Load / Add / Update / Delete / Clear.

Notes
- Uses Microsoft.Data.SqlClient (recommended for .NET 8).
- Parameterized queries, returns SCOPE_IDENTITY() on insert.
- Errors are shown with message boxes.

Troubleshooting
- If you see network/auth errors, verify instance name, DB existence, and Windows authentication.
- If using SQL auth, change the connection string accordingly.