using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp;

namespace EntityFrameworkClassLibrary.Repository
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<Todo> GetTodoById(string id);
        Task AddTodo(Todo todo);
        Task<IActionResult> UpdateTodo(Todo todo);
        Task<IActionResult> DeleteTodo(string id);
        void Update(Todo todo);
        void Save();
    }
}
