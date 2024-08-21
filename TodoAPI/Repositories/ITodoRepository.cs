﻿using TodoAPI.Entities;

namespace TodoAPI.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoEntity>> GetTodoEntities();

    Task<TodoEntity?> GetTodoEntity(int id);

    Task AddTodo(TodoEntity entity);

    Task<bool> SaveChanges();
}
