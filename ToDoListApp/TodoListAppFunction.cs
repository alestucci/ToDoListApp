using EntityFrameworkClassLibrary.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        [ApiExplorerSettings(GroupName = "testee")]

        [FunctionName("Db_GetAllTodos")]
        //[QueryStringParameter("Id", "Required todo's Id", Required = false)]
        [OpenApiOperation(operationId: "GetAllTodos", tags: new[] { "Get the todo list" })]
        //[OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "The **ID** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        public async Task<IActionResult> GetAllTodosFromDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alltodos")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            IEnumerable<Todo> todos = await _unitOfWork.todo.GetAllTodos();
            _unitOfWork.Dispose();
            return new OkObjectResult(todos);

        }

        [FunctionName("Db_GetTodoById")]
        //[QueryStringParameter("Id", "Required todo's Id", Required = false)]
        [OpenApiOperation(operationId: "GetTodoById", tags: new[] { "Get the todo by ID" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        public async Task<IActionResult> GetTodoFromDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Todo todo = await _unitOfWork.todo.GetTodoById(id);
            _unitOfWork.Dispose();
            return new OkObjectResult(todo);

        }

        [FunctionName("Db_CreateTodo")]
        [OpenApiOperation(operationId: "NewTodo", tags: new[] { "Create a todo" })]
        [OpenApiRequestBody("application/json", typeof(TodoNew))]
        //[OpenApiParameter(name: "todo", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "New Todo")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
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
        [OpenApiOperation(operationId: "UpdateTodo", tags: new[] { "Update an existing todo" })]
        [OpenApiRequestBody("application/json", typeof(TodoUpdate))]
        public async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Todo modifiedTodo = JsonConvert.DeserializeObject<Todo>(requestBody);

            var response = await _unitOfWork.todo.UpdateTodo(modifiedTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            return response;
        }

        [FunctionName("Db_DeleteTodo")]
        [OpenApiOperation(operationId: "DeleteTodo", tags: new[] { "Delete an existing todo" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID parameter")]
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
