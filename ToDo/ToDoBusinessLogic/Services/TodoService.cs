using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBusinessLogic.DTO;
using ToDoBusinessLogic.Infrastructure;
using ToDoBusinessLogic.Interfaces;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoBusinessLogic.Services
{
    public class TodoService : ITodoService
    {
        IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TodoDTO>> GetTodos()
        {
            var todos = await Database.TodoRep.GetAllTodosAsync();
            return _mapper.Map< IEnumerable < TodoDTO >>(todos);
        }

        public async Task<TodoDTO> GetTodo(int? id)
        {
            if (id == null)
                throw new ValidationException("Not found Todo Id", "");
            var todo = await Database.TodoRep.GetTodoByIdAsync(id.Value);
            if (todo == null)
                throw new ValidationException("Todo not found", "");

            return _mapper.Map<TodoDTO>(todo);
        }

        public async Task<TodoDTO> GetTodoWithPoints(int? id)
        {
            var todo = await Database.TodoRep.GetTodoWithDetailsAsync(id.Value);

            int countPoints = todo.Points.Count;
            int countCompletedPoints = todo.Points.Count(x => x.IsCompleted == true);
            if (countCompletedPoints == countPoints)
            {
                if (todo.Completed != true)
                {
                    todo.Completed = true;
                    Database.TodoRep.UpdateTodo(todo);
                    await Database.Save();
                }
            }
            else
            {
                if (todo.Completed != false)
                {
                    todo.Completed = false;
                    Database.TodoRep.UpdateTodo(todo);
                    await Database.Save();
                }
            }

            return _mapper.Map<TodoDTO>(todo);
        }

        public void AddTodo(TodoDTO todoDto)
        {
            var todo = _mapper.Map<Todo>(todoDto);
            Database.TodoRep.CreateTodo(todo);
            Database.Save();
        }

        public void EditTodo(TodoDTO todoDto)
        {
            var todo = _mapper.Map<Todo>(todoDto);
            Database.TodoRep.UpdateTodo(todo);
            Database.Save();

        }

        public void DeleteTodo(TodoDTO todoDto)
        {
            //var todo = Database.TodoRep.GetTodoByIdAsync(id.Value);
            Database.TodoRep.DeleteTodo(_mapper.Map<Todo>(todoDto));
            Database.Save();
        }

        public void DeleteAllTodos()
        {
            Todo todo = new Todo();
            Database.TodoRep.DeleteAllTodos(todo);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
