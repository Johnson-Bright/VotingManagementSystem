-- VotingMS SQLite schema for DBeaver
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Users (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Username TEXT NOT NULL,
  Email TEXT NOT NULL,
  PasswordHash TEXT NOT NULL,
  Role TEXT NOT NULL,
  Status TEXT NOT NULL,
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Users_Email ON Users(Email);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Users_Username ON Users(Username);

CREATE TABLE IF NOT EXISTS Voters (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER NOT NULL,
  FirstName TEXT,
  LastName TEXT,
  NationalId TEXT NOT NULL,
  DateOfBirth TEXT,
  Gender TEXT,
  Address TEXT,
  Phone TEXT,
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Voters_NationalId ON Voters(NationalId);

CREATE TABLE IF NOT EXISTS Positions (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  Description TEXT,
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS Elections (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Title TEXT NOT NULL,
  Description TEXT,
  StartDate TEXT NOT NULL,
  EndDate TEXT NOT NULL,
  Status TEXT NOT NULL,
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS Candidates (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER,
  ElectionId INTEGER,
  PositionId INTEGER,
  Name TEXT NOT NULL,
  Party TEXT,
  Bio TEXT,
  Manifesto TEXT,
  PhotoPath TEXT,
  Contact TEXT,
  Status TEXT NOT NULL DEFAULT 'Pending',
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE SET NULL,
  FOREIGN KEY(ElectionId) REFERENCES Elections(Id) ON DELETE CASCADE,
  FOREIGN KEY(PositionId) REFERENCES Positions(Id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS Votes (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  VoterId INTEGER NOT NULL,
  UserId INTEGER,
  CandidateId INTEGER NOT NULL,
  ElectionId INTEGER NOT NULL,
  PositionId INTEGER,
  CastAt TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY(VoterId) REFERENCES Voters(Id) ON DELETE CASCADE,
  FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE SET NULL,
  FOREIGN KEY(CandidateId) REFERENCES Candidates(Id) ON DELETE CASCADE,
  FOREIGN KEY(ElectionId) REFERENCES Elections(Id) ON DELETE CASCADE,
  FOREIGN KEY(PositionId) REFERENCES Positions(Id) ON DELETE SET NULL
);
CREATE INDEX IF NOT EXISTS IX_Votes_VoterId ON Votes(VoterId);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Votes_Voter_Election_Position ON Votes(VoterId, ElectionId, PositionId);

CREATE TABLE IF NOT EXISTS Audit_Log (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER,
  Role TEXT,
  Action TEXT NOT NULL,
  Details TEXT,
  IpAddress TEXT,
  CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE SET NULL
);

CREATE VIEW IF NOT EXISTS ResultsView AS
SELECT
  v.ElectionId AS ElectionId,
  v.CandidateId AS CandidateId,
  c.Name AS CandidateName,
  COUNT(*) AS VotesCount
FROM Votes v
JOIN Candidates c ON c.Id = v.CandidateId
GROUP BY v.ElectionId, v.CandidateId;

-- End of schema
