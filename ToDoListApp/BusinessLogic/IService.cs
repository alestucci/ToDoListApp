using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListApp.Dto;

namespace ToDoListApp.BusinessLogic
{
    public interface IService
    {
        Task<List<TodoDto>> GetAllTodos();
        Task<TodoDto> GetTodoById(string id);
        Task AddTodo(Todo todo);
        Task<IActionResult> UpdateTodo(Todo todo);
        Task<IActionResult> DeleteTodo(string id);
    }
}
