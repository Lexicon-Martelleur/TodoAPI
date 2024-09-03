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

    public async Task<(IEnumerable<TodoDTO>, PaginationVO)> GetTodoEntities(
        TodoQueryDTO query
    )
    {
        var (todoEntities, pagingMetaData) = 
            await _repository.GetTodoEntities(query);
        return (
            _mapper.Map<IEnumerable<TodoDTO>>(todoEntities),
            pagingMetaData
        );
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
        TodoDTO todoDTO
    )
    {
        var todoEntity = await _repository.GetTodoEntity(id);
        if (todoEntity == null)
        {
            return null;
        }
        var updatedTodoEntity = _mapper.Map(todoDTO, todoEntity);
        await _repository.SaveChanges();
        return _mapper.Map<TodoDTO>(updatedTodoEntity);
    }

    public async Task<TodoDTO?> PatchTodo(
        int id,
        int claimedUserId,
        TodoDTO todoDTO
    )
    {
        var todoEntity = await _repository.GetTodoEntityWithClaimedId(
            id,
            claimedUserId);

        if (todoEntity == null)
        {
            return null;
        }
        var updatedTodoEntity = _mapper.Map(todoDTO, todoEntity);
        await _repository.SaveChanges();
        return _mapper.Map<TodoDTO>(updatedTodoEntity);
    }

    public async Task DeleteTodo(int id)
    {
        await _repository.DeleteTodo(id);
        await _repository.SaveChanges();
    }
}
