-- SQL Script to create VotingMIS database and tables

CREATE DATABASE VotingMIS;
GO

USE VotingMIS;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Active'
);

CREATE TABLE Voters (
    VoterId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    NationalId NVARCHAR(20) NOT NULL,
    HasVoted BIT DEFAULT 0
);

CREATE TABLE Elections (
    ElectionId INT PRIMARY KEY IDENTITY(1,1),
    ElectionName NVARCHAR(200) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Upcoming'
);

CREATE TABLE Candidates (
    CandidateId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    ElectionId INT FOREIGN KEY REFERENCES Elections(ElectionId),
    Party NVARCHAR(100),
    Photo NVARCHAR(500)
);

CREATE TABLE Votes (
    VoteId INT PRIMARY KEY IDENTITY(1,1),
    VoterId INT FOREIGN KEY REFERENCES Voters(VoterId),
    CandidateId INT FOREIGN KEY REFERENCES Candidates(CandidateId),
    ElectionId INT FOREIGN KEY REFERENCES Elections(ElectionId),
    VoteDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Results (
    ResultId INT PRIMARY KEY IDENTITY(1,1),
    CandidateId INT FOREIGN KEY REFERENCES Candidates(CandidateId),
    TotalVotes INT DEFAULT 0
);