using System.ComponentModel.DataAnnotations;

namespace VotingMIS.Models
{
    public class Vote
    {
        public int VoteId { get; set; }

        public int VoterId { get; set; }

        public Voter Voter { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public int ElectionId { get; set; }

        public Election Election { get; set; }

        public DateTime VoteDate { get; set; }
    }
}