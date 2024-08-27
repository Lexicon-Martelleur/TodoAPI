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
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoService _todoService;

    public TodoController(
        ILogger<TodoController> logger,
        ITodoService todoService
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
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
    public async Task<IActionResult> GetTodo(int id)
    {
        var todo = await _todoService.GetTodoEntity(id);
        
        if (todo == null)
        {
            _logger.LogInformation($"No todo of id={id} find");
            return NotFound();
        }
        return Ok(todo);
    }

    [HttpPost(Name = "CreateTodo")]
    public async Task<ActionResult<TodoDTO>> CreateTodo(TodoDTO todo)
    {
        var createdTodo = await _todoService.AddTodo(todo);
        return CreatedAtRoute("CreateTodo", createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDTO>> UpdateTodo(int id, TodoDTO todo)
    {
        var updatedTodo = await _todoService.UpdateTodo(id, todo);
        if (todo == null)
        {
            _logger.LogInformation($"No todo of id={id} find");
            return NotFound();
        }
        return Ok(updatedTodo);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TodoDTO>> PatchTodo(
        int id,
        JsonPatchDocument<TodoVO> todoPatchDocument
    )
    {
        var todoToPatch = await _todoService.GetTodoEntity(id);
        if (todoToPatch == null)
        {
            _logger.LogInformation($"No todo of id={id} find");
            return NotFound();
        }

        todoPatchDocument.ApplyTo(todoToPatch.Todo, ModelState);
        if (!ModelState.IsValid || !TryValidateModel(todoToPatch))
        {
            return BadRequest(ModelState);
        }

        var todo = await _todoService.PatchTodo(id, todoToPatch);
        if (todo == null) { return NotFound(); }
        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoDTO>> DeleteTodo(int id)
    {
        var todoEntity = await _todoService.GetTodoEntity(id);
        if (todoEntity == null)
        {
            _logger.LogInformation($"No todo of id={id} find");
            return NotFound();
        }
        await _todoService.DeleteTodo(id);
        return Ok(new { id });
    }

}
