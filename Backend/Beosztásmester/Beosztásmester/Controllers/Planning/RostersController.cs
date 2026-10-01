using Microsoft.AspNetCore.Mvc;

namespace Beosztasmester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RostersService : Controller
    {
        [HttpGet]
        public IActionResult getTest()
        {
            
            return Ok("Hello");
        }
            
    }
}
