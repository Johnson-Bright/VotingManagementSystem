using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Will be hashed

        [Required]
        public string Role { get; set; } // Admin, Voter, Candidate

        public string Status { get; set; } = "Active";
    }
}