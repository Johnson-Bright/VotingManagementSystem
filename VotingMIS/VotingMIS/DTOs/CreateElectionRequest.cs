namespace VotingMIS.DTOs
{
    public class CreateElectionRequest
    {
        public string ElectionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}