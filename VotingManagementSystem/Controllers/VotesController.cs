using Microsoft.AspNetCore.Mvc;
using VotingManagementSystem.Services;
using VotingManagementSystem.Models;

namespace VotingManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotingService _svc;
        public VotesController(IVotingService svc) { _svc = svc; }

        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _svc.GetCandidatesAsync();
            return Ok(candidates);
        }

        // userId is the User.Id stored in localStorage — we resolve to Voter internally
        [HttpPost("cast")]
        public async Task<IActionResult> Cast([FromBody] CastVoteRequest req)
        {
            if (req.VoterId <= 0 || req.CandidateId <= 0)
                return BadRequest("voterId and candidateId required");

            var ok = await _svc.CastVoteByUserIdAsync(req.VoterId, req.CandidateId);
            if (!ok) return BadRequest("Already voted or invalid candidate");
            return Ok();
        }

        [HttpGet("results")]
        public async Task<IActionResult> Results()
        {
            var results = await _svc.GetResultsAsync();
            var total = await _svc.GetTotalVotesAsync();
            return Ok(new { total, results });
        }

        // voterId param is actually userId from the frontend
        [HttpGet("byvoter/{userId}")]
        public async Task<IActionResult> ByVoter(int userId)
        {
            var votes = await _svc.GetVotesByUserIdAsync(userId);
            return Ok(votes);
        }

        [HttpGet("candidate/{candidateId}/count")]
        public async Task<IActionResult> CandidateCount(int candidateId)
        {
            var c = await _svc.GetCandidateVotesAsync(candidateId);
            return Ok(new { candidateId, count = c });
        }
    }

    public class CastVoteRequest { public int VoterId { get; set; } public int CandidateId { get; set; } }
}
