# VotingMS — Voting Management System

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local instance)
- A browser (Chrome recommended)

---

## How to Run

### 1. Set up the database

Open SQL Server Management Studio (SSMS), connect to `localhost`, and run the script:

```
VotingMIS/VotingMIS/database.sql
```

Then run the seed data script to populate all tables:

```
VotingMIS/VotingMIS/seed.sql
```

### 2. Start the backend API

Open a terminal in the project root and run:

```bash
dotnet run --project VotingMIS/VotingMIS/VotingMIS.csproj
```

Wait until you see:
```
Now listening on: http://localhost:5099
```

### 3. Run the frontend

The frontend is plain HTML and must be served over HTTP (not opened as a file) to avoid browser security restrictions.

**Option A — VS Code Live Server (easiest)**
1. Install the [Live Server](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) extension in VS Code
2. Right-click `VotingSystem/Pages/login.html`
3. Click **Open with Live Server**
4. Browser opens at `http://127.0.0.1:5500/VotingSystem/Pages/login.html`

**Option B — Python**
```bash
python -m http.server 5500
```
Then visit: `http://localhost:5500/VotingSystem/Pages/login.html`

**Option C — Node**
```bash
npx serve .
```
Then navigate to `VotingSystem/Pages/login.html` from the URL it provides.

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
