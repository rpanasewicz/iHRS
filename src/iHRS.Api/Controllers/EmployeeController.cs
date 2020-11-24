using iHRS.Api.Configuration;
using iHRS.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace iHRS.Api.Controllers
{
    [JwtAuth]
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeQueries _queries;

        public EmployeeController(EmployeeQueries queries)
        {
            _queries = queries;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_queries.GetAll());
        }
    }
}
