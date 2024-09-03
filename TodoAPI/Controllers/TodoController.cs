using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Controllers;


/// <summary>
/// Controller class used for endpoint <see cref="Router.Todo"/>.
/// </summary>
[ApiController]
[Route(Router.Todo)]
[ApiVersion(API.MAJOR_VERSION_ONE)]
[Authorize(Policy = Authorization.UserPolicy)]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoService _todoService;
    private readonly IClaimService _claimService;

    /// <summary>
    /// Constructor used to inject dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="todoService"></param>
    /// <param name="claimService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TodoController(
        ILogger<TodoController> logger,
        ITodoService todoService,
        IClaimService claimService
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    /// <summary>
    /// Used to get todo objects specified by the query string.
    /// </summary>
    /// <param name="query">A <see cref="TodoQueryDTO"/></param>
    /// <returns>A <see cref="TodoDTO"/></returns>
    /// <response code="200">With the requested todo objects</response>
    /// <response code="401">If not authenticated</response>
    [HttpGet(Name = "GetTodos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<TodoDTO>>> GetTodos(
        [FromQuery] TodoQueryDTO query
    )
    {
        var (todos, paginationMetaData) = await _todoService.GetTodoEntities(query);
        Response.Headers.Append(
            CustomHeader.Pagination,
            JsonSerializer.Serialize(paginationMetaData)
        );
        return Ok(todos ?? []);
    }

    /// <summary>
    /// Used to get a specified todo by a id parameter.
    /// </summary>
    /// <param name="id">A todo id</param>
    /// <returns>A <see cref="TodoDTO"/></returns>
    /// <response code="200">With the specified todo object</response>
    /// <response code="401">If not authenticated</response>
    /// <response code="404">If not find</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodo(
        [FromRoute] int id)
    {
        var todo = await _todoService.GetTodoEntity(id);
        
        if (todo == null)
        {
            _logger.LogInformation($"Could not find todo with id={id}");
            return NotFound();
        }
        return Ok(todo);
    }

    /// <summary>
    /// Used to create a todo specified from the body.
    /// </summary>
    /// <param name="todo">A <see cref="TodoDTO"/></param>
    /// <returns>A <see cref="TodoDTO"/></returns>
    /// <response code="201">With the created todo object</response>
    /// <response code="401">If not authenticated</response>
    [HttpPost(Name = "CreateTodo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TodoDTO>> CreateTodo(
        [FromBody] TodoDTO todo)
    {
        var validatedUserId = _claimService.GetValidUserIdFromClaims(User, todo);
        if (validatedUserId == null)
        {
            return Unauthorized();
        }

        var createdTodo = await _todoService.AddTodo(todo);
        return CreatedAtRoute("CreateTodo", createdTodo);
    }

    /// <summary>
    /// Used to updated a todo specified from the body.
    /// </summary>
    /// <param name="id">A todo id</param>
    /// <param name="todo">A <see cref="TodoDTO"/></param>
    /// <returns>A <see cref="TodoDTO"/></returns>
    /// <response code="200">With the updated todo object</response>
    /// <response code="401">If not authenticated</response>
    /// <response code="404">If not find</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDTO>> UpdateTodo(
        [FromRoute] int id,
        [FromBody] TodoDTO todo)
    {
        var validatedUserId = _claimService.GetValidUserIdFromClaims(User, todo);
        if (validatedUserId == null)
        {
            return Unauthorized();
        }

        var updatedTodo = await _todoService.UpdateTodo(
            id, validatedUserId.Value, todo);
        
        if (todo == null)
        {
            _logger.LogInformation($"Could not find todo with id={id}");
            return NotFound();
        }
        
        return Ok(updatedTodo);
    }

    /// <summary>
    /// Used to patch a todo specified from the body.
    /// </summary>
    /// <param name="id">A todo id</param>
    /// <param name="todoPatchDocument">A <see cref="JsonPatchDocument"/></param>
    /// <returns>A <see cref="TodoDTO"/></returns>
    /// <response code="200">With the patched todo object</response>
    /// <response code="401">If not authenticated</response>
    /// <response code="404">If not find</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoDTO>> PatchTodo(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<TodoVO> todoPatchDocument
    )
    {
        var validatedUserId = _claimService.GetValidUserIdFromClaims(User);
        if (validatedUserId == null)
        {
            return Unauthorized();
        }

        bool patchFunction(TodoDTO? todo)
        {
            if (todo == null) { return false; }
            todoPatchDocument.ApplyTo(todo.Todo, ModelState);
            return ModelState.IsValid && TryValidateModel(todo);
        }

        var todoToPatchWith = await _todoService.GetTodoEntityToPatchWith(
            id,
            validatedUserId.Value,
            patchFunction);

        if (todoToPatchWith == null)
        {
            _logger.LogInformation($"Could not find todo with id={id}");
            return NotFound();
        }

        var isPacthed = await _todoService.PatchTodo(id, validatedUserId.Value, todoToPatchWith);
        if (!isPacthed)
        { 
            return NotFound();
        }
        
        return Ok(todoToPatchWith);
    }

    /// <summary>
    /// Used to delete a todo specified from the id parameter.
    /// </summary>
    /// <param name="id">A todo id</param>
    /// <returns>A <see cref="TodoDeleteDTO"/></returns>
    /// <response code="200">With the deleted id object</response>
    /// <response code="401">If not authenticated</response>
    /// <response code="404">If not find</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoDeleteDTO>> DeleteTodo(
        [FromRoute] int id)
    {
        var validatedUserId = _claimService.GetValidUserIdFromClaims(User);
        if (validatedUserId == null)
        {
            return Unauthorized();
        }

        var todo = await _todoService.GetTodoEntityWithClaimedId(
            id,
            validatedUserId.Value);
        
        if (todo == null)
        {
            _logger.LogInformation($"Could not find todo with id={id}");
            return Unauthorized();
        }

        await _todoService.DeleteTodo(id);
        return Ok(new TodoDeleteDTO(){ Id = id });
    }
}
