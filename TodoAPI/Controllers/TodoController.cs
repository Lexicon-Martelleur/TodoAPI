using Microsoft.AspNetCore.Mvc;

using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route(Router.TODOS)]
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
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetTodos()
        {
            var todos = await _todoService.GetTodoEntities();
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
            var updatedTodo = await _todoService.UpdateTodo(id, todo.Todo);
            if (todo == null)
            {
                _logger.LogInformation($"No todo of id={id} find");
                return NotFound();
            }
            return Ok(updatedTodo);
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
}
