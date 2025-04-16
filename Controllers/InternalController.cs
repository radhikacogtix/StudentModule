using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalController : ControllerBase
    {
        [HttpGet("server-status")]
        public IActionResult GetServerStatus()
        {
            return Ok(new
            {
                Status = "Healthy",
                Time = DateTime.Now
            });
        }
    }
}


