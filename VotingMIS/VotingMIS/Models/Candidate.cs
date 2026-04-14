using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ElectionId { get; set; }

        public Election Election { get; set; }

        public string? Party { get; set; }

        public string? Photo { get; set; } // URL or path
    }
}