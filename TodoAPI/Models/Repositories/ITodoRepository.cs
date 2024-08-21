using TodoAPI.Entities;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoEntity>> GetTodoEntities();

    Task<TodoEntity?> GetTodoEntity(int id);

    Task AddTodo(TodoEntity todo);

    // TodoEntity UpdateTodo(TodoVO todoVO, TodoEntity todo);

    Task DeleteTodo(int id);

    Task<bool> SaveChanges();
}
