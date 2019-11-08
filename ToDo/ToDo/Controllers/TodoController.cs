using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using ToDoBusinessLogic.DTO;
using ToDoBusinessLogic.Interfaces;
using ToDoPersistence.EF;

namespace ToDo.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        ITodoService todoService;

        public TodoController(ITodoService todoserv)
        {
            todoService = todoserv;
        }
        
        [HttpGet]
        //[Route("/all")]
        public IEnumerable<TodoViewModel> GetAllTodos()
        {
            IEnumerable<TodoDTO> todoDtos = todoService.GetTodos();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoDTO, TodoViewModel>()).CreateMapper();
            var todos = mapper.Map<IEnumerable<TodoDTO>, List<TodoViewModel>>(todoDtos);

            return todos;
        }

        [HttpGet("{id}")]
        public TodoViewModel GetTodoById(int id)
        {
            TodoDTO todoDto = todoService.GetTodo(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoDTO, TodoViewModel>()).CreateMapper();
            var todo = mapper.Map<TodoDTO, TodoViewModel>(todoDto);

            return todo;
        }

        [HttpPost]
        public IActionResult Post([FromBody]TodoViewModel todoModel)
        {
            if (ModelState.IsValid)
            {
                var todoDto = new TodoDTO { TodoId = todoModel.TodoId, Title = todoModel.Title, Completed = todoModel.Completed, CreationDate = todoModel.CreationDate };
                
                todoService.AddTodo(todoDto);
                
                return Ok(todoModel);

                
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TodoViewModel todoModel)
        {
            if (ModelState.IsValid)
            {
                var todoDto = new TodoDTO { TodoId = todoModel.TodoId, Title = todoModel.Title, Completed = todoModel.Completed, CreationDate = todoModel.CreationDate };
                todoService.EditTodo(todoDto);
                return Ok(todoModel);
            }
            return BadRequest(ModelState);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            TodoDTO todoDto = todoService.GetTodo(id);
            if (todoDto != null)
            {
                todoService.DeleteTodo(id);

            }
            return Ok(todoDto);
        }
        /*
        //[Route("/deleteall")]
        [HttpDelete("{id}")]
        public void DeleteAllTodos(int id)
        {   
            todoService.DeleteAllTodos();
        }*/
    }
}