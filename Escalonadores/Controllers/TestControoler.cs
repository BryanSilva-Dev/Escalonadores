using Microsoft.AspNetCore.Mvc;

namespace Escalonadores.Controllers
{
    [Route("/")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This service is online now!");
        }
    }
}

