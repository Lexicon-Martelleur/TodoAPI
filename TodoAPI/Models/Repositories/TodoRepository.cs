using Microsoft.EntityFrameworkCore;
using TodoAPI.DBContext;
using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Services;

namespace TodoAPI.Models.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;
    public TodoRepository(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// A combined method for searching and filtering using
    /// deferred execution
    /// </summary>
    /// <param name="author"></param>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public async Task<(IEnumerable<TodoEntity>, PaginationMetaData)> GetTodoEntities(
        TodoQueryDTO query
    )
    {
        var todoCollection = _context.Todos as IQueryable<TodoEntity>;

        if (!string.IsNullOrWhiteSpace(query.Author))
        {
            var trimedAuthor = query.Author.Trim();
            todoCollection = todoCollection.Where(item => item.Author == trimedAuthor);
        }

        if (query.Done != null)
        {
            todoCollection = todoCollection.Where(item => item.Done == query.Done);
        }

        if (!string.IsNullOrWhiteSpace(query.SearchQuery))
        {
            var trimedSearchQuery = query.SearchQuery.Trim();
            todoCollection = todoCollection.Where(item => 
                item.Description.Contains(trimedSearchQuery) ||
                item.Title.Contains(trimedSearchQuery));
        }

        var tottalItemCount = await todoCollection.CountAsync();

        var paginationMetatData = new PaginationMetaData(
            tottalItemCount,
            query.PageSize,
            query.PageNr
        );

        var todos = await todoCollection
            .Skip(query.PageSize * (query.PageNr - 1))
            .Take(query.PageSize)
            .ToListAsync();

        return (todos, paginationMetatData);
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
