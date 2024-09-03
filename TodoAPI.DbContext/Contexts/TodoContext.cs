using Microsoft.EntityFrameworkCore;
using TodoAPI.Entities;

namespace TodoAPI.DbContext.Contexts;

public class TodoContext : Microsoft.EntityFrameworkCore.DbContext
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
