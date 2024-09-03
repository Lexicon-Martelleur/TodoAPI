using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Controllers;

[Route(Router.TODOS)]
[ApiController]
[Authorize(Policy = Authorization.UserPolicy)]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoService _todoService;
    private readonly IClaimService _claimService;

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

    [HttpGet(Name = "GetTodos")]
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

    [HttpGet("{id}")]
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

    [HttpPost(Name = "CreateTodo")]
    public async Task<ActionResult<TodoDTO>> CreateTodo(
        [FromBody] TodoDTO todo)
    {
        var createdTodo = await _todoService.AddTodo(todo);
        return CreatedAtRoute("CreateTodo", createdTodo);
    }

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

    [HttpPatch("{id}")]
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


    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoDTO>> DeleteTodo(
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
        return Ok(new { id });
    }
}
