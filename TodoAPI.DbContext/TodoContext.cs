using Microsoft.EntityFrameworkCore;
using TodoAPI.Entities;

namespace TodoAPI.DBContext;

public class TodoContext : DbContext
{
    public DbSet<TodoEntity> Todos { get; set; }

    public DbSet<UserAuthenticationEntity> UserAuthentications { get; set; }

    public TodoContext() { }

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAuthenticationEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserAuthenticationEntity>()
            .HasIndex(u => u.UserName)
            .IsUnique();
    }
}
