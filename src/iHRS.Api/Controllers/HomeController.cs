using Microsoft.AspNetCore.Mvc;

namespace iHRS.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        public IActionResult Get()
            => Ok("iHRS.Api");
    }
}
