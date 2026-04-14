USE VotingMis;

-- =====================
-- USERS (Voters & Candidates)
-- =====================
INSERT INTO dbo.Users (FullName, Email, Password, Role, Status) VALUES
('James Carter',  'james.carter@email.com',  '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Voter',     'Active'),
('Maria Santos',  'maria.santos@email.com',  '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Voter',     'Active'),
('David Reyes',   'david.reyes@email.com',   '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Voter',     'Active'),
('Sofia Nguyen',  'sofia.nguyen@email.com',  '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Voter',     'Active'),
('Liam Patel',    'liam.patel@email.com',    '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Candidate', 'Active'),
('Aisha Malik',   'aisha.malik@email.com',   '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Candidate', 'Active'),
('Carlos Mendez', 'carlos.mendez@email.com', '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Candidate', 'Active'),
('Emily Zhang',   'emily.zhang@email.com',   '$2a$11$NLFckkBCOa4XRUVVFwSTyOvJRGlcG3xc.K.ctMrbgXucWV7EbfO8y', 'Candidate', 'Active');

-- =====================
-- VOTERS (link to Users)
-- UserId 1,2 already exist (kk kn, test test1)
-- New voters: James=4, Maria=5, David=6, Sofia=7
-- =====================
INSERT INTO dbo.Voters (UserId, NationalId, HasVoted) VALUES
(4, '1990-JC-00123', 0),
(5, '1992-MS-00456', 0),
(6, '1988-DR-00789', 0),
(7, '1995-SN-00321', 0);

-- =====================
-- ELECTIONS
-- =====================
INSERT INTO dbo.Elections (ElectionName, StartDate, EndDate, Status) VALUES
('Student Council General Election 2026',  '2026-04-01 08:00:00', '2026-04-30 20:00:00', 'Active'),
('Faculty Representative Election 2026',   '2026-05-01 08:00:00', '2026-05-15 20:00:00', 'Upcoming'),
('Sports Captain Election 2025',           '2025-10-01 08:00:00', '2025-10-15 20:00:00', 'Closed');

-- =====================
-- CANDIDATES
-- Liam=8, Aisha=9, Carlos=10, Emily=11
-- Election 1 = Student Council, Election 3 = Sports (closed)
-- =====================
INSERT INTO dbo.Candidates (UserId, ElectionId, Party, Photo) VALUES
(8,  1, 'Progressive Students Alliance', NULL),
(9,  1, 'Unity Forward Party',           NULL),
(10, 1, 'Independent',                   NULL),
(11, 3, 'Sports Committee',              NULL);

-- =====================
-- VOTES (for closed/active elections)
-- Voters 1,2 (existing) and 3,4,5,6 voted in Election 1
-- VoterId 1=kk, 2=test, 3=James, 4=Maria, 5=David, 6=Sofia
-- CandidateId 1=Liam, 2=Aisha, 3=Carlos
-- =====================
INSERT INTO dbo.Votes (VoterId, CandidateId, ElectionId, VoteDate) VALUES
(1, 1, 1, '2026-04-05 10:15:00'),
(2, 2, 1, '2026-04-06 11:30:00'),
(3, 1, 1, '2026-04-07 09:45:00'),
(4, 3, 1, '2026-04-08 14:00:00'),
(5, 2, 1, '2026-04-09 16:20:00'),
(6, 1, 1, '2026-04-10 08:55:00');

-- Mark those voters as HasVoted
UPDATE dbo.Voters SET HasVoted = 1 WHERE VoterId IN (1,2,3,4,5,6);

-- =====================
-- RESULTS (tally per candidate)
-- Liam: 3 votes, Aisha: 2 votes, Carlos: 1 vote
-- =====================
INSERT INTO dbo.Results (CandidateId, TotalVotes) VALUES
(1, 3),
(2, 2),
(3, 1),
(4, 0);
