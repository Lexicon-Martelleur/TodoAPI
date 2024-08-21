using TodoAPI.Models.DTO;

namespace TodoAPI.Models.Services
{
    public interface ITodoService
    {
        Task<TodoDTO> AddTodo(TodoDTO todo);
        Task DeleteTodo(int id);
        Task<IEnumerable<TodoDTO>> GetTodoEntities();
        Task<TodoDTO?> GetTodoEntity(int id);
        Task<TodoDTO?> UpdateTodo(int id, TodoDTO todoVO);
        Task<TodoDTO?> PatchTodo(int id, TodoDTO todoVO);
    }
}