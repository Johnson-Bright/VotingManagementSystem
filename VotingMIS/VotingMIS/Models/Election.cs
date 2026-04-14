using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class Election
    {
        public int ElectionId { get; set; }

        [Required]
        public string ElectionName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Upcoming"; // Upcoming, Active, Closed

        // Navigation properties
        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}