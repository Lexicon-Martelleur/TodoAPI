using Microsoft.EntityFrameworkCore;
using TodoAPI.DBContext;
using TodoAPI.Entities;

namespace TodoAPI.Models.Repositories;

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

    /// <summary>
    /// A combined method for searching and filtering using
    /// deferred execution
    /// </summary>
    /// <param name="author"></param>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TodoEntity>> GetTodoEntities(
        string? author,
        string? searchQuery
    )
    {
        if (author == null && searchQuery == null)
        {
            return await GetTodoEntities();
        }

        var collection = _context.Todos as IQueryable<TodoEntity>;

        if (!string.IsNullOrWhiteSpace(author))
        {
            author = author.Trim();
            collection = collection.Where(item => item.Author == author);

        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(item => 
                item.Description.Contains(searchQuery) ||
                item.Title.Contains(searchQuery));
        }

        return await collection
            .OrderBy(item => item.Author)
            .ToListAsync();
    }

    public async Task<TodoEntity?> GetTodoEntity(int id)
    {
        return await _context.Todos
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddTodo(TodoEntity todo)
    {
        await _context.Todos.AddAsync(todo);
        await SaveChanges();
    }

    public async Task DeleteTodo(int id)
    {
        var todoEntity = await _context.Todos
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();

        if (todoEntity == default)
        {
            return;
        }
        _context.Todos.Remove(todoEntity);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() >= 1;
    }
}
