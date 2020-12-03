using iHRS.Api.Configuration;
using iHRS.Application.Commands.Employees;
using iHRS.Application.Common;
using iHRS.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
    [JwtAuth]
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeQueries _queries;
        private readonly ICommandDispatcher _commandDispatcher;

        public EmployeeController(EmployeeQueries queries, ICommandDispatcher commandDispatcher)
        {
            _queries = queries;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_queries.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateEmployeeCommand cmd)
        {
            await _commandDispatcher.SendAsync(cmd);
            return Ok();
        }
    }
}
