#  VoteMS — Voting Management System

> A secure, full-stack web-based voting platform built with ASP.NET Core 10, SQLite, and vanilla JavaScript.



##  Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Project Structure](#project-structure)
- [Database Design](#database-design)
- [User Roles](#user-roles)
- [API Reference](#api-reference)
- [Pages & Routes](#pages--routes)
- [Getting Started](#getting-started)
- [Default Credentials](#default-credentials)
- [Seeded Data](#seeded-data)
- [Security](#security)
- [Reports](#reports)



## Overview

**VoteMS** is a full-stack, web-based Voting Management System designed to conduct secure, transparent, and fair elections online. It supports multiple user roles, real-time results, candidate management, and full administrative control — all through a clean, modern web interface built without any JavaScript framework.

Built as an academic/institutional voting platform demonstrating:
- REST API design
- Relational database modeling
- Role-based access control
- Frontend interactivity with vanilla JS
- Automated PDF and CSV report generation



## Tech Stack

| Layer | Technology | Purpose |
|---|---|---|
| Backend | ASP.NET Core 10 (C#) | REST API, business logic, data access |
| ORM | Entity Framework Core 10 | Database modeling, migrations, queries |
| Database | SQLite | Lightweight file-based relational database |
| DB Tool | DBeaver 26 | Visual database management |
| Frontend | HTML5, CSS3, Vanilla JavaScript | All UI pages — no framework |
| Fonts | Google Fonts (Syne, DM Sans) | Typography |
| PDF Reports | jsPDF + jsPDF-AutoTable | Client-side PDF generation |
| IDE | Visual Studio 2022 | Development environment |
| Platform | Windows / .NET 10 | Runtime environment |
| Architecture | MVC + Service Layer | Separation of concerns |



## Features

###  Security
- SHA-256 password hashing — passwords never stored in plain text
- Role-based page access — each dashboard checks stored role before loading
- Input validation on all forms with clear error messages
- CORS policy configured for development

###  Voting Integrity
- One vote per voter per election — enforced by composite unique DB constraint
- Votes are linked to Voter record (not just User) for traceability
- Candidates must be approved by admin before appearing on the ballot
- Election status (`Draft` / `Open` / `Closed`) controls when voting is allowed

###  Reports (Admin)
- **Election Summary** — all elections with status, candidate count, winner — PDF + CSV
- **Voter Turnout** — all registered voters with details — PDF + CSV
- **Candidate Performance** — votes per candidate across all elections — PDF + CSV
- **Audit Log** — system activity log with date filter — PDF
- All PDFs include a branded header (VoteMS blue) and footer

###  Pagination (Admin)
All admin tables use numbered page navigation (`← 1 2 3 →`) showing current range and total (e.g. `1–3 of 9`). Applied to: Dashboard elections, Dashboard voters, Elections page, Voters page, Candidates pending table, and Candidates all table.



## Project Structure

```
VoteMS/
├── Controllers/          # AuthController, AdminController, ElectionsController, VotesController
├── Services/             # IVotingService, VotingService, IAdminService, AdminService
├── Models/               # User, Voter, Election, Candidate, Position, Vote, AuditLog
├── Data/                 # VotingContext (EF Core DbContext)
├── Migrations/           # InitialCreate migration + ModelSnapshot
├── wwwroot/              # All frontend HTML, CSS, JavaScript files
│   └── Pages/            # All application pages
├── Program.cs            # App startup, DI registration, database seeding, middleware
├── appsettings.json      # SQLite connection string configuration
└── VotingMS.db           # SQLite database file
```


## Database Design

The database consists of **7 tables** managed by Entity Framework Core.

### Tables

| Table | Key Fields | Description |
|---|---|---|
| `Users` | Id, Username, Email, PasswordHash, Role, Status, CreatedAt | Authentication and role management |
| `Voters` | Id, UserId, FirstName, LastName, NationalId, DateOfBirth, Gender, Address, Phone | Voter personal profile linked to User |
| `Elections` | Id, Title, Description, StartDate, EndDate, Status, CreatedAt | Election metadata and lifecycle |
| `Positions` | Id, Name, Description, CreatedAt | Named positions (President, VP, Secretary, etc.) |
| `Candidates` | Id, UserId, ElectionId, PositionId, Name, Party, Bio, Manifesto, Status | Candidate profiles with approval status |
| `Votes` | Id, VoterId, CandidateId, ElectionId, PositionId, CastAt | Vote records with timestamps |
| `AuditLogs` | Id, UserId, Role, Action, Details, IpAddress, CreatedAt | System activity logging |

### Key Constraints & Indexes

- Unique index on `Users.Email` and `Users.Username`
- Unique index on `Voters.NationalId` — prevents duplicate registrations
- **Composite unique index on `(VoterId, ElectionId, PositionId)`** — prevents double voting
- Foreign keys with cascade delete: `Votes → Candidates`, `Votes → Elections`, `Voters → Users`
- Foreign keys with set-null: `Candidates → Users`, `Candidates → Positions`
- SQL View: `ResultsView` — aggregates vote counts per candidate per election

### Relationships

```
User          1 ──── 1   Voter
User          1 ──── N   Candidates
Election      1 ──── N   Candidates
Election      1 ──── N   Votes
Voter         1 ──── N   Votes
Position      1 ──── N   Candidates
```



## User Roles

###  Voter
- Register with National ID, personal details, and password
- Browse all active and closed elections
- Cast exactly one vote per election (enforced at DB level)
- Apply as a candidate directly from any election card
- View live election results at any time
- Receive a vote receipt after voting (election name, candidate, timestamp)

###  Candidate
- Register with party affiliation, manifesto, position, and target election
- Application is reviewed by admin before appearing on the ballot
- View personal dashboard: approved elections, vote count, current ranking, vote share percentage
- Can also vote in elections as a regular voter
- Edit profile: bio, manifesto, party, contact details

###  Administrator
- Full control over the entire system
- Create, edit, and delete elections
- Approve or reject candidate applications
- View, edit, and remove voter accounts
- View live results for all elections
- Generate and download PDF and CSV reports
- Access paginated tables for Elections, Voters, and Candidates



## API Reference

### Authentication — `/api/auth`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/register` | Register new user (Voter or Candidate) |
| `POST` | `/api/auth/login` | Login and receive `userId` + `role` |

### Elections — `/api/elections`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/elections` | Get all elections (public) |
| `GET` | `/api/elections/{id}/candidates` | Get approved candidates for an election |
| `GET` | `/api/elections/{id}/results` | Get ranked results for an election |

### Votes — `/api/votes`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/votes/cast` | Cast a vote (`voterId` + `candidateId`) |
| `GET` | `/api/votes/byvoter/{userId}` | Get all votes cast by a user |
| `GET` | `/api/votes/candidate/{id}/count` | Get vote count for a candidate |
| `GET` | `/api/votes/results` | Get global vote results |

### Admin — `/api/admin`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/admin/stats` | Dashboard statistics |
| `GET/POST/PUT/DELETE` | `/api/admin/elections/{id}` | Full CRUD for elections |
| `GET/PUT/DELETE` | `/api/admin/voters/{id}` | Full CRUD for voters |
| `GET/POST/PUT/DELETE` | `/api/admin/candidates/{id}` | Full CRUD for candidates |
| `POST` | `/api/admin/candidates/approve/{id}` | Approve a candidate |
| `POST` | `/api/admin/candidates/reject/{id}` | Reject a candidate |
| `GET/POST/PUT/DELETE` | `/api/admin/positions/{id}` | Full CRUD for positions |
| `GET/DELETE` | `/api/admin/users/{id}` | View and delete users |



## Pages & Routes

| Page | URL | Role | Description |
|---|---|---|---|
| Home | `/index.html` | Public | Landing page with live stats, active elections, How it works |
| Login | `/Pages/login.html` | Public | Email/password login with role selector |
| Register | `/Pages/register.html` | Public | Full registration form for Voter or Candidate |
| Voter Dashboard | `/Pages/voter-dashboard.html` | Voter/Candidate | Active/closed elections, vote status, Apply as Candidate |
| Cast Vote | `/Pages/cast-vote.html` | Voter/Candidate | Browse candidates, select, confirm and submit |
| Vote Success | `/Pages/vote-success.html` | Voter/Candidate | Vote receipt with election, candidate, timestamp |
| Results | `/Pages/results.html` | All roles | Live ranked results with progress bars and winner highlight |
| Candidate Dashboard | `/Pages/candidate-dashboard.html` | Candidate | Rankings, vote share, elections to vote in |
| Candidate Profile | `/Pages/candidate-profile.html` | Candidate | Edit bio, manifesto, party, personal details |
| Admin Dashboard | `/Pages/admin-dashboard.html` | Admin | Stats overview, active elections, recent voters |
| Admin Elections | `/Pages/admin-elections.html` | Admin | Full CRUD for elections with paginated table |
| Admin Voters | `/Pages/admin-voters.html` | Admin | Search + paginated voter list with edit and remove |
| Admin Candidates | `/Pages/admin-candidates.html` | Admin | Pending approvals + all candidates, filter by election |
| Admin Reports | `/Pages/admin-reports.html` | Admin | PDF/CSV export for all report types |



## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any compatible IDE)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/VoteMS.git
   cd VoteMS
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open in browser**
   ```
   https://localhost:{port}/index.html
   ```

> On first run, the database is automatically seeded with default admin credentials, a sample election, positions, and candidates.



## Default Credentials

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@votems.com` | `admin123` |

>  Change the admin password immediately in a production environment.



## Seeded Data

On first run, the system automatically creates:

- **1 Admin account** — `admin@votems.com`
- **1 Sample election** — *Student Council President 2025* (Status: Open)
- **3 Positions** — President, Vice President, Secretary
- **3 Sample candidates** — Tech Innovation Club, Drama & Arts Society, Environmental Green Club


## System Workflows

### Voter
```
Register → Login → Voter Dashboard → Browse Elections
→ Cast Vote → Vote Receipt → View Live Results
```

### Candidate
```
Register (select election + position) → Pending Approval
→ Admin Approves → Appears on Ballot
→ Login → Candidate Dashboard (vote count + ranking)
→ Can also vote as a regular voter
```

### Administrator
```
Login → Admin Dashboard → Create Elections
→ Approve/Reject Candidates → Monitor Live Results
→ Close Election → Generate PDF/CSV Reports
```



## Security

- **Passwords** are hashed with SHA-256 and never stored in plain text
- **Duplicate votes** are prevented at both application and database level
- **Role-based access** — each dashboard verifies stored role before loading content
- **Audit logs** record all system activity with timestamps and IP addresses
- **Input validation** on all forms with descriptive error messages




