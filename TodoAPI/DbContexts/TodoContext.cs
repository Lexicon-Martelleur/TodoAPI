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
            new TodoEntity
            {
                Id = 1,
                Title = "title1",
                Author = "author1",
                Description = "description1",
                TimeStamp = "1724162544",
                Done = false,
            },
            new TodoEntity
            {
                Id = 2,
                Title = "title2",
                Author = "author2",
                Description = "description2",
                TimeStamp = "1724162544",
                Done = false,
            },
            new TodoEntity
            {
                Id = 3,
                Title = "title3",
                Author = "author3",
                Description = "description3",
                TimeStamp = "1724162544",
                Done = false,
            },
            new TodoEntity
            {
                Id = 4,
                Title = "title4",
                Author = "author4",
                Description = "description4",
                TimeStamp = "1724162544",
                Done = false,
            }
        );
    }
}
