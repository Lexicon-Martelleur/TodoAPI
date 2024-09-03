using Microsoft.AspNetCore.Mvc;
using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;
using Asp.Versioning;

namespace TodoAPI.Controllers;

/// <summary>
/// Controller class used for endpoint <see cref="Router.Authenticate"/>.
/// </summary>
[ApiVersion(API.MAJOR_VERSION_ONE)]
[Route(Router.Authenticate)]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationService _service;
    private ILogger<TodoController> _logger;

    /// <summary>
    /// Constructor used to inject dependencies.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AuthenticationController(
        IAuthenticationService service,
        ILogger<TodoController> logger
        )
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Authenticate user by user credentials;
    /// Password, Email, and User name.
    /// </summary>
    /// <param name="authentication"></param>
    /// <returns>A <see cref="TokenDTO"/></returns>
    /// <response code="200">With the newly created token object</response>
    /// <response code="401">If the authentication fails</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    //[HttpPost]
    //public async Task<ActionResult<TokenDTO>> Logout(
    //    [FromBody] UserAuthenticationDTO authentication)
    //{

    //    throw new NotImplementedException();
    //}

    //[HttpPost]
    //public async Task<ActionResult<TokenDTO>> RegisterAccount(
    //    [FromBody] UserAuthenticationDTO authentication)
    //{

    //    throw new NotImplementedException();
    //}

    //[HttpPut]
    //public async Task<ActionResult<TokenDTO>> UpdateAccount(
    //    [FromBody] UserAuthenticationDTO authentication)
    //{

    //    throw new NotImplementedException();
    //}

    //[HttpDelete]
    //public async Task<ActionResult<TokenDTO>> DeleteAccount(
    //    [FromBody] UserAuthenticationDTO authentication)
    //{

    //    throw new NotImplementedException();
    //}
}
