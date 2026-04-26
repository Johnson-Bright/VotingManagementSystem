# VoteMS — Voting Management System

A full-stack web-based voting management system built with **ASP.NET Core 10**, **SQLite**, and **Vanilla JavaScript**. Supports multiple user roles, real-time results, candidate management, and administrative control.



## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 10 (C#) |
| ORM | Entity Framework Core 10 |
| Database | SQLite |
| DB Tool | DBeaver |
| Frontend | HTML5, CSS3, Vanilla JavaScript |
| Reports | jsPDF + jsPDF-AutoTable |
| IDE | Visual Studio 2022 |


## Features

### Three User Roles
- **Admin** — Full control: manage elections, approve candidates, view results, generate reports
- **Voter** — Register, browse elections, cast votes, apply as candidate
- **Candidate** — Apply to elections, track vote count and ranking, vote in elections

### Core Functionality
- Role-based authentication with SHA-256 password hashing
- Create and manage elections (Draft / Open / Closed)
- Candidate application workflow with admin approval
- One vote per voter per election (enforced at DB level)
- Live election results with ranked candidates and progress bars
- PDF and CSV report generation (Election Summary, Voter Turnout, Candidate Performance, Audit Log)
- Paginated admin tables (Elections, Voters, Candidates)
- Vote receipt after successful submission



## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)


### Default Admin Login
```
Email:    admin@votems.com
Password: admin123
```



## Project Structure

```
VotingManagementSystem/
├── Controllers/          # API controllers (Auth, Admin, Elections, Votes)
├── Services/             # Business logic (VotingService, AdminService)
├── Models/               # Entity models (User, Voter, Election, Candidate, Vote...)
├── Data/                 # EF Core DbContext
├── Migrations/           # Database migrations
├── wwwroot/              # Frontend (HTML, CSS, JS)
│   ├── index.html        # Home page
│   ├── styles.css        # Global styles
│   └── Pages/            # All application pages
├── Program.cs            # App startup and configuration
└── appsettings.json      # Configuration (SQLite path)
```


## Database Schema

| Table | Description |
|---|---|
| Users | Authentication — email, password hash, role |
| Voters | Personal details — name, National ID, DOB, address |
| Elections | Title, description, start/end dates, status |
| Candidates | Name, party, bio, manifesto, approval status |
| Positions | Named positions (President, VP, Secretary...) |
| Votes | Voter → Candidate → Election with timestamp |
| AuditLogs | System activity logging |


## Pages

| Page | Description |
|---|---|
| Home | Landing page with live stats and active elections |
| Login / Register | Authentication with role selection |
| Voter Dashboard | Browse elections, vote, apply as candidate |
| Cast Vote | Select candidate and confirm vote |
| Results | Live ranked results for all elections |
| Candidate Dashboard | Track elections, ranking, and vote share |
| Admin Dashboard | Stats overview with paginated tables |
| Admin Elections | Full CRUD for elections |
| Admin Voters | Search and manage voter accounts |
| Admin Candidates | Approve/reject/manage candidates |
| Admin Reports | Generate PDF/CSV reports |




