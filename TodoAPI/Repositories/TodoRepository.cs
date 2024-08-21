using Microsoft.EntityFrameworkCore;
using TodoAPI.DBContext;
using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.ValueObject;

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



    // TODO: Can i use void here instead of async/Task
    // due Add is an in memory ops instead of an I/= ops?
    public async Task AddTodo(TodoEntity todo)
    {
        await _context.Todos.AddAsync(todo);
        await SaveChanges();
    }

    public async Task<TodoEntity?> UpdateTodo(
        TodoVO todoValues,
        TodoEntity todo
    )
    {
        todo.Id = todo.Id;
        todo.TimeStamp = todo.TimeStamp;
        todo.Title = todoValues.Title;
        todo.Author = todoValues.Author;
        todo.Description = todoValues.Description;
        todo.Done = todoValues.Done;
        await SaveChanges();
        return todo;
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
        await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 1);
    }
}
