using Microsoft.AspNetCore.Mvc;
using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;

namespace TodoAPI.Controllers;

[Route(Router.AUTHENTICATE)]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationService _service;
    private ILogger<TodoController> _logger;

    public AuthenticationController(
        IAuthenticationService service,
        ILogger<TodoController> logger
        ) {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
}

    [HttpPost]
    public async Task<ActionResult<TokenDTO>> Authenticate (
        [FromBody] UserAuthenticationDTO authentication)
    {

        var token = await _service.AuthenticateByPassword(authentication);
        if (token == null)
        {
            Unauthorized();
        }
        return Ok(token);
    }
}
