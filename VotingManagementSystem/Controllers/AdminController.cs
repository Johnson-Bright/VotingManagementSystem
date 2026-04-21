using Microsoft.AspNetCore.Mvc;
using VotingManagementSystem.Services;
using VotingManagementSystem.Models;

namespace VotingManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _admin;
        public AdminController(IAdminService admin) { _admin = admin; }

        // ── Stats ──
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats() => Ok(await _admin.GetStatsAsync());

        // ── Voters ──
        [HttpGet("voters")]
        public async Task<IActionResult> GetVoters() => Ok(await _admin.GetVotersAsync());

        [HttpGet("voters/{id}")]
        public async Task<IActionResult> GetVoter(int id)
        {
            var v = await _admin.GetVoterAsync(id);
            return v == null ? NotFound() : Ok(v);
        }

        [HttpPut("voters/{id}")]
        public async Task<IActionResult> UpdateVoter(int id, [FromBody] Voter updated)
        {
            await _admin.UpdateVoterAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("voters/{id}")]
        public async Task<IActionResult> DeleteVoter(int id)
        {
            await _admin.DeleteVoterAsync(id);
            return NoContent();
        }

        // ── Candidates ──
        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates() => Ok(await _admin.GetCandidatesAsync());

        [HttpGet("candidates/{id}")]
        public async Task<IActionResult> GetCandidate(int id)
        {
            var c = await _admin.GetCandidateAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost("candidates/approve/{id}")]
        public async Task<IActionResult> ApproveCandidate(int id)
        {
            await _admin.ApproveCandidateAsync(id);
            return Ok();
        }

        [HttpPost("candidates/reject/{id}")]
        public async Task<IActionResult> RejectCandidate(int id)
        {
            await _admin.RejectCandidateAsync(id);
            return Ok();
        }

        [HttpPost("candidates")]
        public async Task<IActionResult> AddCandidate([FromBody] Candidate c)
        {
            await _admin.AddCandidateAsync(c);
            return Ok(c);
        }

        [HttpPut("candidates/{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] Candidate updated)
        {
            await _admin.UpdateCandidateAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("candidates/{id}")]
        public async Task<IActionResult> RemoveCandidate(int id)
        {
            await _admin.RemoveCandidateAsync(id);
            return NoContent();
        }

        // ── Elections ──
        [HttpGet("elections")]
        public async Task<IActionResult> GetElections() => Ok(await _admin.GetElectionsAsync());

        [HttpGet("elections/{id}")]
        public async Task<IActionResult> GetElection(int id)
        {
            var e = await _admin.GetElectionAsync(id);
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost("elections")]
        public async Task<IActionResult> CreateElection([FromBody] Election e)
        {
            var created = await _admin.CreateElectionAsync(e);
            return Ok(created);
        }

        [HttpPut("elections/{id}")]
        public async Task<IActionResult> UpdateElection(int id, [FromBody] Election updated)
        {
            await _admin.UpdateElectionAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("elections/{id}")]
        public async Task<IActionResult> DeleteElection(int id)
        {
            await _admin.DeleteElectionAsync(id);
            return NoContent();
        }

        // ── Positions ──
        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions() => Ok(await _admin.GetPositionsAsync());

        [HttpPost("positions")]
        public async Task<IActionResult> CreatePosition([FromBody] Position p)
        {
            var created = await _admin.CreatePositionAsync(p);
            return Ok(created);
        }

        [HttpPut("positions/{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] Position updated)
        {
            await _admin.UpdatePositionAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("positions/{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            await _admin.DeletePositionAsync(id);
            return NoContent();
        }

        // ── Users ──
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers() => Ok(await _admin.GetUsersAsync());

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _admin.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
