using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.AspNetCore.Http.Internal;
using System.Linq;
using ToDoListApp.Models;
using Microsoft.SqlServer.Server;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ToDoListApp
{
    public static class Function1
    {
        [FunctionName("Db_GetTodo")]
        public static IActionResult GetTodoFromDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tododb/{id?}")] HttpRequest req, ILogger log, string id,
            [Sql(@"IF (@id IS NULL or @id = '') SELECT * FROM [ToDo].[dbo].[Todos] ELSE SELECT * FROM [ToDo].[dbo].[Todos] WHERE Id = @id", Parameters = "@id={id}", CommandType = System.Data.CommandType.Text, ConnectionStringSetting = "ConnectionString")] IEnumerable<Todo> todos)
        {

            return new OkObjectResult(todos);
        }

        [FunctionName("Db_CreateTodo")]
        public static async Task<IActionResult> CreateTodoDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tododb")] HttpRequest req, ILogger log,
            [Sql("Todos", ConnectionStringSetting = "ConnectionString")] IAsyncCollector<Todo> todos)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Todo>(requestBody);

            await todos.AddAsync(input);
            await todos.FlushAsync();

            return new OkResult();
        }

        [FunctionName("Cache_GetTodos")]
        public static IActionResult GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting todo list items");
            return new OkObjectResult(TodoApiInMemory.todoList);
        }

        [FunctionName("Cache_GetTodoById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            var todo = TodoApiInMemory.todoList.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todo);
        }

        [FunctionName("Cache_CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new todo list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            TodoApiInMemory.todoList.Add(todo);
            return new OkObjectResult(todo);

        }


        [FunctionName("Cache_UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            var todo = TodoApiInMemory.todoList.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;

            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                todo.TaskDescription = updated.TaskDescription;
            }

            return new OkObjectResult(todo);
        }

        [FunctionName("Cache_DeleteTodo")]
        public static IActionResult DeleteTodo([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            var todo = TodoApiInMemory.todoList.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            TodoApiInMemory.todoList.Remove(todo);
            return new OkResult();
        }

    }
}
