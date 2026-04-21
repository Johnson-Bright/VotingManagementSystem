using Microsoft.AspNetCore.Mvc;
using VotingManagementSystem.Data;
using VotingManagementSystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace VotingManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VotingContext _db;
        public AuthController(VotingContext db) { _db = db; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("email and password required");

            if (_db.Users.Any(u => u.Email == req.Email)) return BadRequest("email taken");

            var user = new User
            {
                Username = req.Email.Split('@')[0],
                Email = req.Email,
                PasswordHash = Hash(req.Password),
                Role = req.Role ?? "Voter",
                Status = "Active"
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var voter = new Voter
            {
                UserId = user.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                NationalId = req.NationalId ?? string.Empty,
                DateOfBirth = req.DateOfBirth,
                Address = req.Address,
                Phone = req.Phone
            };
            _db.Voters.Add(voter);

            if (string.Equals(user.Role, "Candidate", StringComparison.OrdinalIgnoreCase))
            {
                var candidate = new Candidate
                {
                    UserId = user.Id,
                    Name = req.FirstName + " " + req.LastName,
                    Party = req.Party,
                    Manifesto = req.Manifesto,
                    ElectionId = req.ElectionId,
                    PositionId = req.PositionId,
                    Status = "Pending"
                };
                _db.Candidates.Add(candidate);
            }

            await _db.SaveChangesAsync();
            return Ok(new { userId = user.Id, role = user.Role });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == req.Email);
            if (user == null) return Unauthorized();
            if (user.PasswordHash != Hash(req.Password)) return Unauthorized();
            return Ok(new { userId = user.Id, role = user.Role });
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public class RegisterRequest
    {
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? NationalId { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        // candidate fields
        public string? Party { get; set; }
        public string? Manifesto { get; set; }
        public int? ElectionId { get; set; }
        public int? PositionId { get; set; }
    }

    public class LoginRequest { public string Email { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }
}
