using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingMIS.Data;

namespace VotingMIS.Controllers
{
    [ApiController]
    [Route("api/elections")]
    [Authorize]
    public class ElectionController : ControllerBase
    {
        private readonly VotingDbContext _context;
        public ElectionController(VotingDbContext context) => _context = context;

        // GET api/elections — all elections with candidate count & vote count
        [HttpGet]
        public IActionResult GetAll()
        {
            var elections = _context.Elections
                .Select(e => new {
                    e.ElectionId, e.ElectionName, e.StartDate, e.EndDate, e.Status,
                    CandidateCount = e.Candidates.Count(),
                    VoteCount = e.Votes.Count()
                })
                .OrderByDescending(e => e.ElectionId)
                .ToList();
            return Ok(elections);
        }

        // GET api/elections/{id} — single election with candidates + user info
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var election = _context.Elections
                .Where(e => e.ElectionId == id)
                .Select(e => new {
                    e.ElectionId, e.ElectionName, e.StartDate, e.EndDate, e.Status,
                    VoteCount = e.Votes.Count(),
                    Candidates = e.Candidates.Select(c => new {
                        c.CandidateId, c.Party, c.Photo,
                        User = new { c.User.UserId, c.User.FullName, c.User.Email }
                    }).ToList()
                })
                .FirstOrDefault();

            if (election == null) return NotFound();
            return Ok(election);
        }
    }
}
