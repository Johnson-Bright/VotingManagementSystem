using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public int TotalVotes { get; set; }
    }
}