using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using TodoAPI.Constants;
using TodoAPI.Models.DTO;
using TodoAPI.Models.ValueObject;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route(Router.TODOS)]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoStore _todoStore;
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodoController(
            ILogger<TodoController> logger,
            TodoStore todoStore,
            ITodoRepository todoRepository,
            IMapper mapper
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _todoStore = todoStore ?? throw new ArgumentNullException(nameof(todoStore));
            _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetTodos")]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetTodos()
        {
            var todoEntities = await _todoRepository.GetTodoEntities();
            var todos = _mapper.Map<IEnumerable<TodoDTO>>(todoEntities);
            return Ok(todos ?? []);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            // var todo = _todoStore.Todos.FirstOrDefault(item => item.Id == id);
            var todoEntity = await _todoRepository.GetTodoEntity(id);
            
            if (todoEntity == null)
            {
                _logger.LogInformation($"No todo of id={id} find");
                return NotFound();
            }
            return Ok(_mapper.Map<TodoDTO>(todoEntity));
        }
    }
}
