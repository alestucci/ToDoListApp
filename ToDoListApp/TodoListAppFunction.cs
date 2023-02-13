using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using EntityFrameworkClassLibrary.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
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

        [SwaggerIgnore]
        [FunctionName("SwaggerJson")]
        public static Task<HttpResponseMessage> SwaggerJson(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/json")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerJsonDocumentResponse(req));
        }

        //[SwaggerIgnore]
        //[FunctionName("SwaggerYaml")]
        //public static Task<HttpResponseMessage> SwaggerYaml(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/yaml")]
        //    HttpRequestMessage req,
        //    [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        //{
        //    return Task.FromResult(swashBuckleClient.CreateSwaggerYamlDocumentResponse(req));
        //}

        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public static Task<HttpResponseMessage> SwaggerUi(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/ui")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        }

        [FunctionName("Db_GetTodo")]
        //[QueryStringParameter("Id", "Required todo's Id", Required = false)]
        [OpenApiOperation(operationId: "GetTodo", tags: new[] { "Get the todo list or a todo by ID" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "The **ID** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        public async Task<IActionResult> GetTodoFromDb(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            if (String.IsNullOrEmpty(id) || id == "undefined" || id == "{id}")
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
        [OpenApiOperation(operationId: "NewTodo", tags: new[] { "Create a todo" })]
        [OpenApiRequestBody("application/json", typeof(Todo))]
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
        [OpenApiRequestBody("application/json", typeof(Todo))]
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
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "The **ID** parameter")]
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
