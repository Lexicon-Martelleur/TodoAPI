using TodoAPI.Entities;

namespace TodoAPI.Models.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoEntity>> GetTodoEntities();

    Task<TodoEntity?> GetTodoEntity(int id);

    Task AddTodo(TodoEntity todo);

    Task DeleteTodo(int id);

    Task<bool> SaveChanges();
}
