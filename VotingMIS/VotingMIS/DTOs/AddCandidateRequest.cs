namespace VotingMIS.DTOs
{
    public class AddCandidateRequest
    {
        public int UserId { get; set; }
        public int ElectionId { get; set; }
        public string Party { get; set; }
    }
}
