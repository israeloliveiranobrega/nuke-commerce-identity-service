using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NukeAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DebugController : ControllerBase
    {
        [Authorize(Policy = "RequireUser")]
        [HttpGet("User")]
        public async Task<IActionResult> Userr()
        {
            return Ok("Conteúdo para user");
        }

        [Authorize(Policy = "RequireVerifiedUser")]
        [HttpGet("VerifiedUser")]
        public async Task<IActionResult> VerifiedUser()
        {
            return Ok("Conteúdo para VerifiedUser");
        }

        [Authorize(Policy = "RequireSupport")]
        [HttpGet("Support")]
        public async Task<IActionResult> Support()
        {
            return Ok("Conteúdo para Support");
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> Admin()
        {
            return Ok("Conteúdo para Admin");
        }

        [Authorize(Policy = "RequireMaster")]
        [HttpGet("Master")]
        public async Task<IActionResult> Master()
        {
            return Ok ("Conteúdo para Master");
        }
    }
}
