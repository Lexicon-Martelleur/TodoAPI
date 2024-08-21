using TodoAPI.Models.DTO;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Services
{
    public interface ITodoService
    {
        Task<TodoDTO> AddTodo(TodoDTO todo);
        Task DeleteTodo(int id);
        Task<IEnumerable<TodoDTO>> GetTodoEntities();
        Task<TodoDTO?> GetTodoEntity(int id);
        Task<TodoDTO?> UpdateTodo(int id, TodoVO todoVO);
        Task<TodoDTO?> PatchTodo(int id, TodoVO todoVO);
    }
}