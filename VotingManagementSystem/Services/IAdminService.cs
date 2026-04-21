using VotingManagementSystem.Models;

namespace VotingManagementSystem.Services
{
    public interface IAdminService
    {
        // Voters
        Task<IEnumerable<Voter>> GetVotersAsync();
        Task<Voter?> GetVoterAsync(int id);
        Task UpdateVoterAsync(int id, Voter updated);
        Task DeleteVoterAsync(int id);

        // Candidates
        Task<IEnumerable<Candidate>> GetCandidatesAsync();
        Task<Candidate?> GetCandidateAsync(int id);
        Task ApproveCandidateAsync(int candidateId);
        Task RejectCandidateAsync(int candidateId);
        Task AddCandidateAsync(Candidate c);
        Task UpdateCandidateAsync(int id, Candidate updated);
        Task RemoveCandidateAsync(int candidateId);

        // Elections
        Task<IEnumerable<Election>> GetElectionsAsync();
        Task<Election?> GetElectionAsync(int id);
        Task<Election> CreateElectionAsync(Election e);
        Task UpdateElectionAsync(int id, Election updated);
        Task DeleteElectionAsync(int id);

        // Positions
        Task<IEnumerable<Position>> GetPositionsAsync();
        Task<Position> CreatePositionAsync(Position p);
        Task UpdatePositionAsync(int id, Position updated);
        Task DeletePositionAsync(int id);

        // Users
        Task<IEnumerable<User>> GetUsersAsync();
        Task DeleteUserAsync(int id);

        // Stats
        Task<AdminStats> GetStatsAsync();
    }

    public class AdminStats
    {
        public int TotalElections { get; set; }
        public int ActiveElections { get; set; }
        public int TotalVoters { get; set; }
        public int TotalCandidates { get; set; }
        public int PendingCandidates { get; set; }
        public int TotalVotes { get; set; }
    }
}