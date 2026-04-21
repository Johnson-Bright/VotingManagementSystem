using Microsoft.EntityFrameworkCore;
using VotingManagementSystem.Data;
using VotingManagementSystem.Models;

namespace VotingManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly VotingContext _db;
        public AdminService(VotingContext db) { _db = db; }

        // ── Voters ──
        public async Task<IEnumerable<Voter>> GetVotersAsync() =>
            await _db.Voters.AsNoTracking().ToListAsync();

        public async Task<Voter?> GetVoterAsync(int id) =>
            await _db.Voters.FindAsync(id);

        public async Task UpdateVoterAsync(int id, Voter updated)
        {
            var v = await _db.Voters.FindAsync(id);
            if (v == null) return;
            v.FirstName = updated.FirstName;
            v.LastName = updated.LastName;
            v.NationalId = updated.NationalId;
            v.DateOfBirth = updated.DateOfBirth;
            v.Gender = updated.Gender;
            v.Address = updated.Address;
            v.Phone = updated.Phone;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteVoterAsync(int id)
        {
            var v = await _db.Voters.FindAsync(id);
            if (v == null) return;
            // also delete the linked user
            var u = await _db.Users.FindAsync(v.UserId);
            if (u != null) _db.Users.Remove(u);
            else _db.Voters.Remove(v);
            await _db.SaveChangesAsync();
        }

        // ── Candidates ──
        public async Task<IEnumerable<Candidate>> GetCandidatesAsync() =>
            await _db.Candidates.AsNoTracking().ToListAsync();

        public async Task<Candidate?> GetCandidateAsync(int id) =>
            await _db.Candidates.FindAsync(id);

        public async Task ApproveCandidateAsync(int candidateId)
        {
            var c = await _db.Candidates.FindAsync(candidateId);
            if (c == null) return;
            c.Status = "Approved";
            await _db.SaveChangesAsync();
        }

        public async Task RejectCandidateAsync(int candidateId)
        {
            var c = await _db.Candidates.FindAsync(candidateId);
            if (c == null) return;
            c.Status = "Rejected";
            await _db.SaveChangesAsync();
        }

        public async Task AddCandidateAsync(Candidate c)
        {
            c.Status = c.Status ?? "Approved";
            _db.Candidates.Add(c);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCandidateAsync(int id, Candidate updated)
        {
            var c = await _db.Candidates.FindAsync(id);
            if (c == null) return;
            c.Name = updated.Name;
            c.Party = updated.Party;
            c.Bio = updated.Bio;
            c.Manifesto = updated.Manifesto;
            c.Contact = updated.Contact;
            c.ElectionId = updated.ElectionId;
            c.PositionId = updated.PositionId;
            c.Status = updated.Status;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveCandidateAsync(int candidateId)
        {
            var c = await _db.Candidates.FindAsync(candidateId);
            if (c == null) return;
            _db.Candidates.Remove(c);
            await _db.SaveChangesAsync();
        }

        // ── Elections ──
        public async Task<IEnumerable<Election>> GetElectionsAsync() =>
            await _db.Elections.AsNoTracking().ToListAsync();

        public async Task<Election?> GetElectionAsync(int id) =>
            await _db.Elections.FindAsync(id);

        public async Task<Election> CreateElectionAsync(Election e)
        {
            e.CreatedAt = DateTime.UtcNow;
            _db.Elections.Add(e);
            await _db.SaveChangesAsync();
            return e;
        }

        public async Task UpdateElectionAsync(int id, Election updated)
        {
            var e = await _db.Elections.FindAsync(id);
            if (e == null) return;
            e.Title = updated.Title;
            e.Description = updated.Description;
            e.StartDate = updated.StartDate;
            e.EndDate = updated.EndDate;
            e.Status = updated.Status;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteElectionAsync(int id)
        {
            var e = await _db.Elections.FindAsync(id);
            if (e == null) return;
            _db.Elections.Remove(e);
            await _db.SaveChangesAsync();
        }

        // ── Positions ──
        public async Task<IEnumerable<Position>> GetPositionsAsync() =>
            await _db.Positions.AsNoTracking().ToListAsync();

        public async Task<Position> CreatePositionAsync(Position p)
        {
            p.CreatedAt = DateTime.UtcNow;
            _db.Positions.Add(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task UpdatePositionAsync(int id, Position updated)
        {
            var p = await _db.Positions.FindAsync(id);
            if (p == null) return;
            p.Name = updated.Name;
            p.Description = updated.Description;
            await _db.SaveChangesAsync();
        }

        public async Task DeletePositionAsync(int id)
        {
            var p = await _db.Positions.FindAsync(id);
            if (p == null) return;
            _db.Positions.Remove(p);
            await _db.SaveChangesAsync();
        }

        // ── Users ──
        public async Task<IEnumerable<User>> GetUsersAsync() =>
            await _db.Users.AsNoTracking().ToListAsync();

        public async Task DeleteUserAsync(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return;
            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
        }

        // ── Stats ──
        public async Task<AdminStats> GetStatsAsync()
        {
            return new AdminStats
            {
                TotalElections = await _db.Elections.CountAsync(),
                ActiveElections = await _db.Elections.CountAsync(e => e.Status == "Open"),
                TotalVoters = await _db.Voters.CountAsync(),
                TotalCandidates = await _db.Candidates.CountAsync(),
                PendingCandidates = await _db.Candidates.CountAsync(c => c.Status == "Pending"),
                TotalVotes = await _db.Votes.CountAsync()
            };
        }
    }
}