using System.ComponentModel.DataAnnotations;

namespace VotingManagementSystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        // link to Voters table
        public int VoterId { get; set; }
        public int? UserId { get; set; }
        public int CandidateId { get; set; }
        public int ElectionId { get; set; }
        public int? PositionId { get; set; }
        public Candidate? Candidate { get; set; }
        public DateTime CastAt { get; set; } = DateTime.UtcNow;
    }
}