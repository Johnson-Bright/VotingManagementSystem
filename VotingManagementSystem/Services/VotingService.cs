using Microsoft.EntityFrameworkCore;
using VotingManagementSystem.Data;
using VotingManagementSystem.Models;

namespace VotingManagementSystem.Services
{
    public class VotingService : IVotingService
    {
        private readonly VotingContext _db;
        public VotingService(VotingContext db) { _db = db; }

        public async Task<IEnumerable<Candidate>> GetCandidatesAsync()
        {
            return await _db.Candidates.AsNoTracking().ToListAsync();
        }

        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            return await _db.Candidates.FindAsync(id);
        }

        public async Task<bool> CastVoteAsync(int voterId, int candidateId)
        {
            var candidate = await _db.Candidates.FindAsync(candidateId);
            if (candidate == null) return false;

            var existing = await _db.Votes.FirstOrDefaultAsync(v => v.VoterId == voterId && v.ElectionId == candidate.ElectionId && v.PositionId == candidate.PositionId);
            if (existing != null) return false;

            var vote = new Vote { VoterId = voterId, CandidateId = candidateId, ElectionId = candidate.ElectionId ?? 0, PositionId = candidate.PositionId };
            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
            return true;
        }

        // Accepts User.Id, resolves to Voter.Id automatically
        public async Task<bool> CastVoteByUserIdAsync(int userId, int candidateId)
        {
            var voter = await _db.Voters.FirstOrDefaultAsync(v => v.UserId == userId);
            if (voter == null) return false;
            return await CastVoteAsync(voter.Id, candidateId);
        }

        public async Task<int> GetTotalVotesAsync()
        {
            return await _db.Votes.CountAsync();
        }

        public async Task<IEnumerable<Election>> GetElectionsAsync()
        {
            return await _db.Elections.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Candidate>> GetCandidatesByElectionAsync(int electionId)
        {
            return await _db.Candidates.Where(c => c.ElectionId == electionId && c.Status == "Approved").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Vote>> GetVotesByVoterAsync(int voterId)
        {
            return await _db.Votes.Where(v => v.VoterId == voterId).AsNoTracking().ToListAsync();
        }

        // Accepts User.Id, resolves to Voter.Id
        public async Task<IEnumerable<Vote>> GetVotesByUserIdAsync(int userId)
        {
            var voter = await _db.Voters.FirstOrDefaultAsync(v => v.UserId == userId);
            if (voter == null) return Enumerable.Empty<Vote>();
            return await _db.Votes.Where(v => v.VoterId == voter.Id).AsNoTracking().ToListAsync();
        }

        public async Task<int> GetCandidateVotesAsync(int candidateId)
        {
            return await _db.Votes.CountAsync(v => v.CandidateId == candidateId);
        }

        public async Task<Dictionary<int,int>> GetResultsAsync()
        {
            return await _db.Votes
                .GroupBy(v => v.CandidateId)
                .Select(g => new { CandidateId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.CandidateId, v => v.Count);
        }

        public async Task<IEnumerable<CandidateResult>> GetResultsByElectionAsync(int electionId)
        {
            var candidates = await _db.Candidates
                .Where(c => c.ElectionId == electionId)
                .AsNoTracking()
                .ToListAsync();

            var voteCounts = await _db.Votes
                .Where(v => v.ElectionId == electionId)
                .GroupBy(v => v.CandidateId)
                .Select(g => new { CandidateId = g.Key, Count = g.Count() })
                .ToListAsync();

            int total = voteCounts.Sum(v => v.Count);

            return candidates.Select(c =>
            {
                int votes = voteCounts.FirstOrDefault(v => v.CandidateId == c.Id)?.Count ?? 0;
                return new CandidateResult
                {
                    CandidateId = c.Id,
                    Name = c.Name,
                    Party = c.Party,
                    Votes = votes,
                    Percentage = total > 0 ? Math.Round((double)votes / total * 100, 1) : 0
                };
            }).OrderByDescending(r => r.Votes).ToList();
        }
    }
}