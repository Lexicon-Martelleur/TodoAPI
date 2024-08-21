using TodoAPI.Models.DTO;

namespace TodoAPI.Repositories;

public class TodoStore
{
    public List<TodoDTO> Todos { get; set; }

    public static TodoStore Current { get; set; } = new TodoStore();

    public TodoStore()
    {
        Todos = [
            new()
            {
                Id = 1,
                Timestamp = "1724162544",
                Todo = new()
                {
                    Title = "Test 1",
                    Author =   "A1",
                    Description = "D1",
                    Done = false,
                }
            },
            new()
            {
                Id = 1,
                Timestamp = "1724162544",
                Todo = new()
                {
                    Title = "Test 2",
                    Author =   "A2",
                    Description = "D",
                    Done = false,
                }
            },
            new()
            {
                Id = 3,
                Timestamp = "1724162544",
                Todo = new()
                {
                    Title = "Test 3",
                    Author =   "A3",
                    Description = "D3",
                    Done = false,
                }
            }
        ];
    }
}
