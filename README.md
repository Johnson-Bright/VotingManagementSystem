# VotingMS — Voting Management System

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local instance)
- Python 3 or Node.js (to serve the frontend)
- A browser (Chrome recommended)

---

## Database Setup

Open CMD in the project root and run:

```cmd
sqlcmd -S localhost -i VotingMIS\VotingMIS\database.sql
```

Then seed the sample data:

```cmd
sqlcmd -S localhost -d VotingMis -i VotingMIS\VotingMIS\seed.sql
```

> Or open SSMS, connect to `localhost`, and run both SQL files manually.

---

## How to Run

### Step 1 — Start the backend API

Open CMD in the project root and run:

```cmd
dotnet run --project VotingMIS\VotingMIS\VotingMIS.csproj
```

Wait until you see:
```
Now listening on: http://localhost:5099
```
Keep this window open.

### Step 2 — Serve the frontend

Open a **second CMD window** in the project root and run:

```cmd
npx serve VotingSystem
```

Then open your browser at:
```
http://localhost:3000
```

It will automatically redirect to the login page.

> If you prefer Python:
> ```cmd
> cd VotingSystem
> python -m http.server 3000
> ```

---

## Test Accounts

All seeded accounts use the password `Admin@123`.

| Role      | Email                    | Password  |
|-----------|--------------------------|-----------|
| Admin     | admin@votems.com         | Admin@123 |
| Voter     | james.carter@email.com   | Admin@123 |
| Voter     | maria.santos@email.com   | Admin@123 |
| Voter     | david.reyes@email.com    | Admin@123 |
| Voter     | sofia.nguyen@email.com   | Admin@123 |
| Candidate | liam.patel@email.com     | Admin@123 |
| Candidate | aisha.malik@email.com    | Admin@123 |
| Candidate | carlos.mendez@email.com  | Admin@123 |
| Candidate | emily.zhang@email.com    | Admin@123 |

---

## Project Structure

```
VotingManagementSystem/
├── VotingMIS/VotingMIS/        # ASP.NET Core Web API (backend)
│   ├── Controllers/            # API endpoints
│   ├── Models/                 # Database models
│   ├── DTOs/                   # Request/response objects
│   ├── Data/                   # EF Core DbContext
│   ├── database.sql            # Database schema
│   ├── seed.sql                # Sample data
│   └── appsettings.json        # Connection string & JWT config
└── VotingSystem/               # Frontend (HTML/CSS/JS)
    ├── Pages/                  # All UI pages
    └── styles.css              # Global styles
```
