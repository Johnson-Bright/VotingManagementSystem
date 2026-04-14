using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingMIS.Data;
using VotingMIS.DTOs;
using VotingMIS.Models;

namespace VotingMIS.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly VotingDbContext _context;
        public AdminController(VotingDbContext context) => _context = context;

        // ── Dashboard stats ──────────────────────────────────────────────
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var totalElections  = _context.Elections.Count();
            var activeElections = _context.Elections.Count(e => e.Status == "Active");
            var closedElections = _context.Elections.Count(e => e.Status == "Closed");
            var totalVoters     = _context.Users.Count(u => u.Role == "Voter");
            var totalVotes      = _context.Votes.Count();
            var todayVotes      = _context.Votes.Count(v => v.VoteDate.Date == DateTime.Today);
            var totalCandidates = _context.Candidates.Count();

            return Ok(new {
                totalElections, activeElections, closedElections,
                totalVoters, totalVotes, todayVotes, totalCandidates
            });
        }

        // ── Elections ────────────────────────────────────────────────────
        [HttpGet("elections")]
        public IActionResult GetElections()
        {
            var elections = _context.Elections
                .Select(e => new {
                    e.ElectionId, e.ElectionName, e.StartDate, e.EndDate, e.Status,
                    CandidateCount = e.Candidates.Count(),
                    VoteCount      = e.Votes.Count()
                })
                .OrderByDescending(e => e.ElectionId)
                .ToList();
            return Ok(elections);
        }

        [HttpPost("create-election")]
        public async Task<IActionResult> CreateElection([FromBody] CreateElectionRequest request)
        {
            var election = new Election {
                ElectionName = request.ElectionName,
                StartDate    = request.StartDate,
                EndDate      = request.EndDate,
                Status       = "Active"
            };
            _context.Elections.Add(election);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Election created successfully", election.ElectionId });
        }

        [HttpPut("election/{id}/close")]
        public async Task<IActionResult> CloseElection(int id)
        {
            var election = await _context.Elections.FindAsync(id);
            if (election == null) return NotFound();
            election.Status = "Closed";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Election closed" });
        }

        [HttpDelete("election/{id}")]
        public async Task<IActionResult> DeleteElection(int id)
        {
            var election = await _context.Elections.FindAsync(id);
            if (election == null) return NotFound();
            _context.Elections.Remove(election);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Election deleted" });
        }

        // ── Voters ───────────────────────────────────────────────────────
        [HttpGet("voters")]
        public IActionResult GetVoters()
        {
            var voters = _context.Users
                .Where(u => u.Role == "Voter")
                .Select(u => new {
                    u.UserId, u.FullName, u.Email, u.Status,
                    VotesCount = _context.Votes
                        .Count(v => v.Voter.UserId == u.UserId),
                    NationalId = _context.Voters
                        .Where(v => v.UserId == u.UserId)
                        .Select(v => v.NationalId)
                        .FirstOrDefault()
                })
                .OrderByDescending(u => u.UserId)
                .ToList();
            return Ok(voters);
        }

        [HttpPut("approve-voter/{id}")]
        public async Task<IActionResult> ApproveVoter(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role != "Voter") return NotFound();
            user.Status = "Active";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Voter approved" });
        }

        [HttpDelete("voter/{id}")]
        public async Task<IActionResult> DeleteVoter(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Voter removed" });
        }

        // ── Candidates ───────────────────────────────────────────────────
        [HttpGet("candidates")]
        public IActionResult GetCandidates()
        {
            var candidates = _context.Candidates
                .Select(c => new {
                    c.CandidateId, c.Party, c.Photo,
                    c.ElectionId,
                    ElectionName   = c.Election.ElectionName,
                    ElectionStatus = c.Election.Status,
                    User           = new { c.User.UserId, c.User.FullName, c.User.Email },
                    VoteCount      = _context.Votes.Count(v => v.CandidateId == c.CandidateId)
                })
                .OrderByDescending(c => c.CandidateId)
                .ToList();
            return Ok(candidates);
        }

        [HttpPost("add-candidate")]
        public async Task<IActionResult> AddCandidate([FromBody] AddCandidateRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) return NotFound(new { message = "User not found" });

            var candidate = new Candidate {
                UserId     = request.UserId,
                ElectionId = request.ElectionId,
                Party      = request.Party
            };
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Candidate added" });
        }

        [HttpDelete("candidate/{id}")]
        public async Task<IActionResult> RemoveCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null) return NotFound();
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Candidate removed" });
        }

        // ── Results ──────────────────────────────────────────────────────
        [HttpGet("results/{electionId}")]
        public IActionResult GetResults(int electionId)
        {
            var results = _context.Candidates
                .Where(c => c.ElectionId == electionId)
                .Select(c => new {
                    c.CandidateId, c.Party,
                    CandidateName = c.User.FullName,
                    TotalVotes    = _context.Votes.Count(v => v.CandidateId == c.CandidateId)
                })
                .OrderByDescending(r => r.TotalVotes)
                .ToList();
            return Ok(results);
        }

        // ── All users ────────────────────────────────────────────────────
        [HttpGet("all-users")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new { u.UserId, u.FullName, u.Email, u.Role, u.Status })
                .ToList();
            return Ok(users);
        }
    }
}
