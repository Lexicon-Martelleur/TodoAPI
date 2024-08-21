using AutoMapper;

using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Repositories;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;

    public TodoService(
        ITodoRepository repository,
        IMapper mapper
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TodoDTO>> GetTodoEntities()
    {
        var todoEntities = await _repository.GetTodoEntities();
        return _mapper.Map<IEnumerable<TodoDTO>>(todoEntities);
    }

    public async Task<TodoDTO?> GetTodoEntity(int id)
    {
        var todo = await _repository.GetTodoEntity(id);
        if (todo == null) { return null; }
        return _mapper.Map<TodoDTO>(todo);
    }

    public async Task<TodoDTO> AddTodo(TodoDTO todo)
    {
        var todoEntity = _mapper.Map<TodoEntity>(todo);
        await _repository.AddTodo(todoEntity);
        await _repository.SaveChanges();
        return _mapper.Map<TodoDTO>(todoEntity);
    }

    public async Task<TodoDTO?> UpdateTodo(
        int id,
        TodoVO todoValues
    )
    {
        // TODO! Auto Mapping does not work with the profile created due
        // new instance is created.
        // Id is ignored does not work?
        // var updatedTodo = _mapper.Map(todo, todoEntity);
        // await _todoRepository.SaveChanges();
        var todoEntity = await _repository.GetTodoEntity(id);
        if (todoEntity == null)
        {
            return null;
        }
        var updatedTodoEntity = _repository.UpdateTodo(todoValues, todoEntity);
        await _repository.SaveChanges();
        return _mapper.Map<TodoDTO>(updatedTodoEntity);
    }

    public async Task DeleteTodo(int id)
    {
        await _repository.DeleteTodo(id);
        await _repository.SaveChanges();
    }
}
