using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingMIS.Data;
using VotingMIS.DTOs;
using VotingMIS.Models;
using System.Security.Claims;

namespace VotingMIS.Controllers
{
    [ApiController]
    [Route("api/voter")]
    [Authorize(Roles = "Voter")]
    public class VoterController : ControllerBase
    {
        private readonly VotingDbContext _context;
        public VoterController(VotingDbContext context) => _context = context;

        // GET api/voter/me — current voter profile + stats
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user   = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return NotFound();

            var voter = _context.Voters.FirstOrDefault(v => v.UserId == userId);
            var votesCast = voter != null
                ? _context.Votes.Count(v => v.VoterId == voter.VoterId)
                : 0;

            var activeElections = _context.Elections.Count(e => e.Status == "Active");
            var closedElections = _context.Elections.Count(e => e.Status == "Closed");

            return Ok(new {
                user.UserId, user.FullName, user.Email, user.Role,
                VoterId    = voter?.VoterId,
                NationalId = voter?.NationalId,
                VotesCast  = votesCast,
                ActiveElections = activeElections,
                ClosedElections = closedElections
            });
        }

        // GET api/voter/elections — elections with voted status for this voter
        [HttpGet("elections")]
        public IActionResult GetElections()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var voter  = _context.Voters.FirstOrDefault(v => v.UserId == userId);

            var votedElectionIds = voter != null
                ? _context.Votes.Where(v => v.VoterId == voter.VoterId)
                                .Select(v => v.ElectionId).ToHashSet()
                : new HashSet<int>();

            var elections = _context.Elections
                .Select(e => new {
                    e.ElectionId, e.ElectionName, e.StartDate, e.EndDate, e.Status,
                    CandidateCount = e.Candidates.Count(),
                    VoteCount      = e.Votes.Count(),
                    HasVoted       = votedElectionIds.Contains(e.ElectionId)
                })
                .OrderByDescending(e => e.Status == "Active")
                .ThenByDescending(e => e.ElectionId)
                .ToList();

            return Ok(elections);
        }

        // POST api/voter/vote
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] VoteRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var voter  = _context.Voters.FirstOrDefault(v => v.UserId == userId);
            if (voter == null) return NotFound(new { message = "Voter record not found" });

            var existing = _context.Votes.FirstOrDefault(v => v.VoterId == voter.VoterId && v.ElectionId == request.ElectionId);
            if (existing != null) return BadRequest(new { message = "Already voted in this election" });

            var election = _context.Elections.Find(request.ElectionId);
            if (election == null || election.Status != "Active")
                return BadRequest(new { message = "Election is not active" });

            _context.Votes.Add(new Vote {
                VoterId     = voter.VoterId,
                CandidateId = request.CandidateId,
                ElectionId  = request.ElectionId,
                VoteDate    = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Vote cast successfully" });
        }

        // GET api/voter/results — closed elections with results
        [HttpGet("results")]
        public IActionResult GetResults()
        {
            var elections = _context.Elections
                .Where(e => e.Status == "Closed")
                .Select(e => new {
                    e.ElectionId, e.ElectionName, e.EndDate,
                    TotalVotes = e.Votes.Count(),
                    Results = e.Candidates
                        .Select(c => new {
                            c.CandidateId, c.Party,
                            CandidateName = c.User.FullName,
                            TotalVotes    = e.Votes.Count(v => v.CandidateId == c.CandidateId)
                        })
                        .OrderByDescending(r => r.TotalVotes)
                        .ToList()
                })
                .OrderByDescending(e => e.ElectionId)
                .ToList();

            return Ok(elections);
        }
    }
}
