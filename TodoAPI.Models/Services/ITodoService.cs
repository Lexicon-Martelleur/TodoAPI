using TodoAPI.Models.DTO;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Services
{
    public interface ITodoService
    {
        Task<TodoDTO> AddTodo(TodoDTO todo);
        Task DeleteTodo(int id);
        Task<(IEnumerable<TodoDTO>, PaginationVO)> GetTodoEntities(TodoQueryDTO query);
        Task<TodoDTO?> GetTodoEntity(int id);
        Task<TodoDTO?> UpdateTodo(int id, TodoDTO todoVO);
        Task<TodoDTO?> PatchTodo(int id, int claimedUserId, TodoDTO todoVO);
    }
}