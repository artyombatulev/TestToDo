using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
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

        public TodoService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<TodoDTO> GetTodos()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Todo, TodoDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Todo>, List<TodoDTO>>(Database.Todos.GetAll());
        }

        public TodoDTO GetTodo(int? id)
        {
            if (id == null)
                throw new ValidationException("Not found Todo Id", "");
            var todo = Database.Todos.Get(id.Value);
            if (todo == null)
                throw new ValidationException("Todo not found", "");

            return new TodoDTO { TodoId = todo.TodoId, Title = todo.Title, CreationDate = todo.CreationDate, Completed = todo.Completed };
        }

        public void AddTodo(TodoDTO todoDto)
        {
            var todo = new Todo { TodoId = todoDto.TodoId, Title = todoDto.Title, CreationDate = todoDto.CreationDate ?? DateTime.Now, Completed = todoDto.Completed };
            Database.Todos.Create(todo);
            Database.Save();
        }

        public void EditTodo(TodoDTO todoDto)
        {
            var todo = new Todo { TodoId = todoDto.TodoId, Title = todoDto.Title, CreationDate = todoDto.CreationDate ?? DateTime.Now, Completed = todoDto.Completed };

            Database.Todos.Update(todo);
            Database.Save();

        }

        public void DeleteTodo(int? id)
        {
            var todo = Database.Todos.Get(id.Value);
            Database.Todos.Delete(todo);
            Database.Save();
        }

        public void DeleteAllTodos()
        {/*
            IEnumerable<TodoDTO> todoDtos = GetTodos();
            foreach(TodoDTO i in todoDtos)
            {
                var todo = new Todo { TodoId = i.TodoId, Title = i.Title, CreationDate = i.CreationDate ?? DateTime.Now, Completed = i.Completed };
                Database.Todos.Delete(todo.TodoId);
            }*/
            Todo todo = new Todo();
            Database.Todos.DeleteAll(todo);
            Database.Save();
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
