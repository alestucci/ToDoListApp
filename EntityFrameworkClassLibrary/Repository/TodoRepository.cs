using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp;

namespace EntityFrameworkClassLibrary.Repository
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        private ApplicationDbContext _db;
        public TodoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos()
        {
            var todos = await _db.Todos.ToListAsync();

            return todos;
        }

        public async Task<Todo?> GetTodoById(string id)
        {
            Todo? todo = await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
            {
                return null;
            }
            return todo;
        }

        public async Task AddTodo(Todo todo)
        {
            await _db.Todos.AddAsync(todo);
        }

        public async Task<IActionResult> UpdateTodo(Todo todo)
        {
            Todo? dbTodo = await _db.Todos.FindAsync(todo.Id);

            if (dbTodo == null)
                return new NotFoundResult();

            dbTodo.TaskDescription = todo.TaskDescription;
            dbTodo.IsCompleted = todo.IsCompleted;
            _db.Todos.Update(dbTodo);

            return new OkObjectResult(todo);
        }

        public async Task<IActionResult> DeleteTodo(string id)
        {
            Todo? dbTodo = await _db.Todos.FindAsync(id);
            if (dbTodo == null)
                return new NotFoundResult();

            _db.Todos.Remove(dbTodo);

            return new OkObjectResult(dbTodo);
        }


        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Todo todo)
        {
            _db.Todos.Update(todo);
        }
    }
}
