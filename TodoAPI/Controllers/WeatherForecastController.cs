using Microsoft.AspNetCore.Mvc;

using TodoAPI.Constants;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route(Router.TODOS)]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] todos = new[]
        {
            "Todo1", "Todo2", "Todo4", "TODO99"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTodos")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return Ok(todos);
        }
    }
}
