using Microsoft.EntityFrameworkCore;
using TodoAPI.Entities;

namespace TodoAPI.DBContext;

public class TodoContext : DbContext
{
    public DbSet<TodoEntity> Todos { get; set; }

    public TodoContext() { }

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoEntity>().HasData(
            new TodoEntity("1724162544", "title1", "author1", "description1", false)
            {
                Id = 1
            },
            new TodoEntity("1724162544", "title2", "author2", "description2", false)
            {
                Id = 2
            },
            new TodoEntity("1724162544", "title3", "author3", "description3", false)
            {
                Id = 3
            },
            new TodoEntity("1724162544", "title4", "author4", "description4", false)
            {
                Id = 4
            }
        );
    }

}
