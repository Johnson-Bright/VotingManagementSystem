using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingMIS.Data;
using System.Security.Claims;

namespace VotingMIS.Controllers
{
    [ApiController]
    [Route("api/candidate")]
    [Authorize(Roles = "Candidate")]
    public class CandidateController : ControllerBase
    {
        private readonly VotingDbContext _context;
        public CandidateController(VotingDbContext context) => _context = context;

        // GET api/candidate/me — profile + elections + vote stats
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user   = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return NotFound();

            // Pull candidacies into memory first, then compute rank client-side
            var candidacies = _context.Candidates
                .Where(c => c.UserId == userId)
                .Select(c => new {
                    c.CandidateId, c.Party, c.ElectionId,
                    c.Election.ElectionName,
                    c.Election.StartDate,
                    c.Election.EndDate,
                    c.Election.Status,
                    VotesReceived        = _context.Votes.Count(v => v.CandidateId == c.CandidateId),
                    TotalVotesInElection = _context.Votes.Count(v => v.ElectionId == c.ElectionId)
                })
                .ToList();

            // Compute rank in memory
            var result = candidacies.Select(c => {
                var ranked = _context.Candidates
                    .Where(x => x.ElectionId == c.ElectionId)
                    .Select(x => new { x.CandidateId, Votes = _context.Votes.Count(v => v.CandidateId == x.CandidateId) })
                    .ToList()
                    .OrderByDescending(x => x.Votes)
                    .ToList();
                var rank = ranked.FindIndex(x => x.CandidateId == c.CandidateId) + 1;
                return new {
                    c.CandidateId, c.Party, c.ElectionId,
                    c.ElectionName, c.StartDate, c.EndDate, c.Status,
                    c.VotesReceived, c.TotalVotesInElection,
                    Rank = rank
                };
            }).ToList();

            var totalVotes   = result.Sum(c => c.VotesReceived);
            var electionsWon = result.Count(c => c.Status == "Closed" && c.Rank == 1);

            return Ok(new {
                user.UserId, user.FullName, user.Email,
                TotalVotes   = totalVotes,
                ElectionsWon = electionsWon,
                Candidacies  = result
            });
        }
    }
}
