using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using TodoAPI.Constants;
using TodoAPI.Entities;
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
        private readonly TodoStore _todoStore; // In Memory
        private readonly ITodoRepository _todoRepository;  // In Persistent
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

        [HttpPost(Name = "CreateTodo")]
        public async Task<ActionResult<TodoDTO>> CreateTodo(TodoDTO todo)
        {
            var todoEntity = _mapper.Map<TodoEntity>(todo);
            await _todoRepository.AddTodo(todoEntity);
            var createdTodo = _mapper.Map<TodoDTO>(todoEntity);
            return CreatedAtRoute("CreateTodo", createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoDTO>> UpdateTodo(int id, TodoDTO todo)
        {
            var todoEntity = await _todoRepository.GetTodoEntity(id);
            if (todoEntity == null)
            {
                _logger.LogInformation($"No todo of id={id} find");
                return NotFound();
            }

            // TODO! Auto Mapping does not work with the profile created due
            // new instance is created.
            // Id is ignored does not work?
            // var updatedTodo = _mapper.Map(todo, todoEntity);
            // await _todoRepository.SaveChanges();

            var updatedTodo = await _todoRepository.UpdateTodo(todo.Todo, todoEntity);
            return Ok(_mapper.Map<TodoDTO>(updatedTodo));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoDTO>> DeleteTodo(int id)
        {
            var todoEntity = await _todoRepository.GetTodoEntity(id);
            if (todoEntity == null)
            {
                _logger.LogInformation($"No todo of id={id} find");
                return NotFound();
            }
            await _todoRepository.DeleteTodo(id);
            return Ok(new { id });
        }

    }
}
