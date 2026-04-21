using System.ComponentModel.DataAnnotations;

namespace VotingManagementSystem.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ElectionId { get; set; }
        public int? PositionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Party { get; set; }
        public string? Bio { get; set; }
        public string? Manifesto { get; set; }
        public string? PhotoPath { get; set; }
        public string? Contact { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}