using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Repositories;

public interface ITodoRepository
{
    Task<(IEnumerable<TodoEntity>, PaginationVO)> GetTodoEntities(TodoQueryDTO query);

    Task<TodoEntity?> GetTodoEntity(int id);

    Task AddTodo(TodoEntity todo);

    Task DeleteTodo(int id);

    Task<bool> SaveChanges();
}
