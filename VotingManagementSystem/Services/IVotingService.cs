using VotingManagementSystem.Models;

namespace VotingManagementSystem.Services
{
    public interface IVotingService
    {
        Task<IEnumerable<Candidate>> GetCandidatesAsync();
        Task<Candidate?> GetCandidateAsync(int id);
        Task<bool> CastVoteAsync(int voterId, int candidateId);
        Task<bool> CastVoteByUserIdAsync(int userId, int candidateId);
        Task<IEnumerable<Election>> GetElectionsAsync();
        Task<IEnumerable<Candidate>> GetCandidatesByElectionAsync(int electionId);
        Task<IEnumerable<Vote>> GetVotesByVoterAsync(int voterId);
        Task<IEnumerable<Vote>> GetVotesByUserIdAsync(int userId);
        Task<int> GetCandidateVotesAsync(int candidateId);
        Task<int> GetTotalVotesAsync();
        Task<Dictionary<int, int>> GetResultsAsync();
        Task<IEnumerable<CandidateResult>> GetResultsByElectionAsync(int electionId);
    }

    public class CandidateResult
    {
        public int CandidateId { get; set; }
        public string Name { get; set; } = "";
        public string? Party { get; set; }
        public int Votes { get; set; }
        public double Percentage { get; set; }
    }
}
