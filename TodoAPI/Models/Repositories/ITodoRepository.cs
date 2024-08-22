using TodoAPI.Entities;

namespace TodoAPI.Models.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoEntity>> GetTodoEntities();

    Task<IEnumerable<TodoEntity>> GetTodoEntities(string? author, string? searchQuery);

    Task<TodoEntity?> GetTodoEntity(int id);

    Task AddTodo(TodoEntity todo);

    Task DeleteTodo(int id);

    Task<bool> SaveChanges();
}
