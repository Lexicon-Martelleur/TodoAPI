using Microsoft.AspNetCore.Mvc;

using TodoAPI.Constants;
using TodoAPI.Data;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route(Router.TODOS)]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoStore _todoStore;

        public TodoController(
            ILogger<TodoController> logger,
            TodoStore todoStore
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _todoStore = todoStore ?? throw new ArgumentNullException(nameof(todoStore));
        }

        [HttpGet(Name = "GetTodos")]
        public async Task<ActionResult<IEnumerable<string>>> GetTodos()
        {
            return Ok(_todoStore.Todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetTodo(int id)
        {
            var todo = _todoStore.Todos.FirstOrDefault(item => item.Id == id);
            
            if (todo == null)
            {
                _logger.LogInformation($"No todo of id={id} find");
                return NotFound();
            }
            return Ok(todo);
        }
    }
}
