using EntityFrameworkClassLibrary.UnitOfWork;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Dto;

namespace ToDoListApp.BusinessLogic
{
    internal class Service : IService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IMapper _mapper { get; set; }
        public Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddTodo(Todo todo)
        {
            Todo newTodo = _mapper.Map<Todo>(todo);

            await _unitOfWork.TodoRepository.AddTodo(newTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();
        }

        public async Task<IActionResult> DeleteTodo(string id)
        {
            Todo dbTodo = await _unitOfWork.TodoRepository.GetTodoById(id);

            if (dbTodo == null)
                return new NotFoundObjectResult(dbTodo);

            await _unitOfWork.TodoRepository.DeleteTodo(dbTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            TodoDto todoDeleted = _mapper.Map<TodoDto>(dbTodo);

            return new OkObjectResult(todoDeleted);
        }

        public async Task<List<TodoDto>> GetAllTodos()
        {
            List<TodoDto> todoDtos = new List<TodoDto>();

            var todos = await _unitOfWork.TodoRepository.GetAllTodos();

            if (todos == null)
                return null;

            foreach (var todo in todos)
            {
                TodoDto todoDto = _mapper.Map<TodoDto>(todo);
                todoDtos.Add(todoDto);
            }

            return todoDtos;
        }

        public async Task<TodoDto> GetTodoById(string id)
        {
            Todo dbTodo = await _unitOfWork.TodoRepository.GetTodoById(id);

            TodoDto todoMapped = _mapper.Map<TodoDto>(dbTodo);

            return todoMapped;
        }

        public async Task<IActionResult> UpdateTodo(Todo todo)
        {
            Todo dbTodo = await _unitOfWork.TodoRepository.GetTodoById(todo.Id);

            if (dbTodo == null)
                return new NotFoundObjectResult(dbTodo);

            await _unitOfWork.TodoRepository.UpdateTodo(todo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            TodoDto todoUpdated = _mapper.Map<TodoDto>(todo);

            return new OkObjectResult(todoUpdated);
        }
    }
}