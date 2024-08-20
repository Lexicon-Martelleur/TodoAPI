using Microsoft.EntityFrameworkCore;
using TodoAPI.Entities;

namespace TodoAPI.DBContext;

public class TodoContext : DbContext
{
    public DbSet<TodoTimeEntity> TimeStampedTodos { get; set; }
    public DbSet<TodoValueEntity> ValueTodos { get; set; }

    public TodoContext() { }

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoTimeEntity>().HasData(
            new TodoTimeEntity("1724162544")
            {
                Id = 1
            },
            new TodoTimeEntity("1724162544")
            {
                Id = 2
            },
            new TodoTimeEntity("1724162544")
            {
                Id = 3
            },
            new TodoTimeEntity("1724162544")
            {
                Id = 4
            }
        );


        modelBuilder.Entity<TodoValueEntity>().HasData(
            new TodoValueEntity("title1", "author1", "description1", false)
            {
                Id = 1,
                TodoTimeEntityId = 1
            },
            new TodoValueEntity("title2", "author2", "description2", false)
            {
                Id = 2,
                TodoTimeEntityId = 2
            },
            new TodoValueEntity("title3", "author3", "description3", false)
            {
                Id = 3,
                TodoTimeEntityId = 3
            },
            new TodoValueEntity("title4", "author4", "description4", false)
            {
                Id = 4,
                TodoTimeEntityId = 4
            }
        );
        base.OnModelCreating(modelBuilder);


    }

}
