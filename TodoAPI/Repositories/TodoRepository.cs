using Microsoft.EntityFrameworkCore;
using TodoAPI.DBContext;
using TodoAPI.Entities;

namespace TodoAPI.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;
    public TodoRepository(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TodoEntity>> GetTodoEntities()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<TodoEntity?> GetTodoEntity(int id)
    {
        return await _context.Todos
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddTodo(TodoEntity entity)
    {
        await _context.Todos.AddAsync(entity);
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 1);
    }
}
