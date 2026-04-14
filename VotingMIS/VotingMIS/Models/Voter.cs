using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class Voter
    {
        public int VoterId { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public string? NationalId { get; set; }

        public bool HasVoted { get; set; } // Note: This should be per election, but as per plan
    }
}