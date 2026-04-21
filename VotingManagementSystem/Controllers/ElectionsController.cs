using Microsoft.AspNetCore.Mvc;
using VotingManagementSystem.Services;

namespace VotingManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElectionsController : ControllerBase
    {
        private readonly IVotingService _svc;
        public ElectionsController(IVotingService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _svc.GetElectionsAsync());

        [HttpGet("{id}/candidates")]
        public async Task<IActionResult> GetCandidates(int id) =>
            Ok(await _svc.GetCandidatesByElectionAsync(id));

        [HttpGet("{id}/results")]
        public async Task<IActionResult> GetResults(int id) =>
            Ok(await _svc.GetResultsByElectionAsync(id));
    }
}
