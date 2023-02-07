using EntityFrameworkClassLibrary.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

//Startup.cs trigger
[assembly: FunctionsStartup(typeof(ToDoListApp.Startup))]

namespace ToDoListApp
{
    public class TodoListAppFunction
    {
        private readonly IUnitOfWork _unitOfWork;
        public TodoListAppFunction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [FunctionName("Db_GetTodo")]
        public async Task<IActionResult> GetTodoFromDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id?}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            if (id.IsNullOrEmpty())
            {
                IEnumerable<Todo> todos = await _unitOfWork.todo.GetAllTodos();
                _unitOfWork.Dispose();
                return new OkObjectResult(todos);
            } else
            {
                Todo todo = await _unitOfWork.todo.GetTodoById(id);
                _unitOfWork.Dispose();
                return new OkObjectResult(todo);
            }
        }

        [FunctionName("Db_CreateTodo")]
        public async Task<IActionResult> CreateTodoDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Todo newTodo = JsonConvert.DeserializeObject<Todo>(requestBody);

            await _unitOfWork.todo.AddTodo(newTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            log.LogInformation("Todo created: Id = {0}, CreatedTime = {1}, TaskDescription = {2}, IsCompleted = {3}", newTodo.Id, newTodo.CreatedTime, newTodo.TaskDescription, newTodo.IsCompleted);

            return new OkObjectResult(newTodo);
        }

        [FunctionName("Db_UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Todo modifiedTodo = JsonConvert.DeserializeObject<Todo>(requestBody);

            await _unitOfWork.todo.UpdateTodo(modifiedTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            return new OkObjectResult(modifiedTodo);
        }

        [FunctionName("Db_DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _unitOfWork.todo.DeleteTodo(id);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            return new OkResult();
        }
    }
}
