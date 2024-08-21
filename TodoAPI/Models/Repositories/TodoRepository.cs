using Microsoft.EntityFrameworkCore;
using TodoAPI.DBContext;
using TodoAPI.Entities;
using TodoAPI.Models.ValueObject;

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

    //public TodoEntity UpdateTodo(
    //    TodoVO todoVO,
    //    TodoEntity todoEntity
    //)
    //{
    //    todoEntity.Id = todoEntity.Id;
    //    todoEntity.TimeStamp = todoEntity.TimeStamp;
    //    todoEntity.Title = todoVO.Title;
    //    todoEntity.Author = todoVO.Author;
    //    todoEntity.Description = todoVO.Description;
    //    todoEntity.Done = todoVO.Done;
    //    return todoEntity;
    //}

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
