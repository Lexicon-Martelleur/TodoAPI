using Microsoft.EntityFrameworkCore;
using TodoAPI.DbContext.Contexts;
using TodoAPI.Entities;
using TodoAPI.Lib;

namespace TodoAPI.DbContext.Seeds;

public class SeedTodoDB
{
    public static async Task RunAsync(
        TodoContext context,
        ISecurityService securityService)
    {
        if (await IsExistingMovieData(context))
        {
            return;
        }

        Console.WriteLine("Seeding Data...");
        var users = CreateUsers(securityService);
        await context.AddRangeAsync(users);

        var todos = CreateTodoEntitites(users);
        await context.AddRangeAsync(todos);

        await context.SaveChangesAsync();
    }

    private static async Task<bool> IsExistingMovieData(
        TodoContext context)
    {
        return await context.UserAuthentications.AnyAsync();
    }

    private static IEnumerable<TodoEntity> CreateTodoEntitites(
        IEnumerable<UserAuthenticationEntity> users)
    {
        List<TodoEntity> todos = [];

        foreach (var user in users)
        {
            List<TodoEntity> userTodos = [
                new TodoEntity
                {
                    Title = "title1",
                    Author = user.UserName,
                    Description = "description1",
                    TimeStamp = "1724162544",
                    Done = false,
                    UserAuthenticationId = user.Id,
                    UserAuthentication = user
                },
                new TodoEntity
                {
                    Title = "title2",
                    Author = user.UserName,
                    Description = "description2",
                    TimeStamp = "1724162544",
                    Done = false,
                    UserAuthenticationId = user.Id,
                    UserAuthentication = user
                },
                new TodoEntity
                {
                    Title = "title3",
                    Author = user.UserName,
                    Description = "description3",
                    TimeStamp = "1724162544",
                    Done = false,
                    UserAuthenticationId = user.Id,
                    UserAuthentication = user
                },
                new TodoEntity
                {
                    Title = "title4",
                    Author = user.UserName,
                    Description = "description4",
                    TimeStamp = "1724162544",
                    Done = false,
                    UserAuthenticationId = user.Id,
                    UserAuthentication = user
                }
            ];
            todos.AddRange(userTodos);
        }
        return todos;
    }

    private static IEnumerable<UserAuthenticationEntity> CreateUsers(
        ISecurityService securityService)
    {
        List<UserAuthenticationEntity> users = [
            new()
            {
                // Id = Guid.NewGuid().ToString(),
                UserName = "User1",
                Password = "password",
                Email = "user1@mail.com",
                Todos = []
            },
            new()
            {
                // Id = Guid.NewGuid().ToString(),
                UserName = "User2",
                Password = "password",
                Email = "user2@mail.com",
                Todos = []
            },
            new()
            {
                // Id = Guid.NewGuid().ToString(),
                UserName = "User3",
                Password = "password",
                Email = "user3@mail.com",
                Todos = []
            }
        ];

        foreach (var user in users)
        {
            var hashedPwd = securityService.HashPassword(user, user.Password);
            user.Password = hashedPwd;
        }

        return users;
    }
}
