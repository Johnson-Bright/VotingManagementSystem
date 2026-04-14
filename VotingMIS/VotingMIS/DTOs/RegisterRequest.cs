namespace VotingMIS.DTOs
{
    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Voter or Candidate
        public string NationalId { get; set; } // For Voter
    }
}