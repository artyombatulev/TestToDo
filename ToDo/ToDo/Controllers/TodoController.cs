using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TodoController> _logger;
        private readonly IMapper _mapper;
        ITodoService todoService;

        public TodoController(ITodoService todoserv, ILogger<TodoController> logger, IMapper mapper)
        {
            todoService = todoserv;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoViewModel>> GetAllTodos()
        {

            var todoDtos = await todoService.GetTodos();
            _logger.LogInformation($"Returned all todos from database.");

            return _mapper.Map<IEnumerable<TodoViewModel>>(todoDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> GetTodoById(int id)
        {

            try
            {
                var todo = await todoService.GetTodo(id);
                if (todo == null)
                {
                    _logger.LogError($"Todo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned todo with id: {id}");

                    var todoResult = _mapper.Map<TodoViewModel>(todo);
                    return todoResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTodoById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public ActionResult<TodoViewModel> PostTodo([FromBody]TodoViewModel todoModel)
        {
            try
            {
                if (todoModel == null)
                {
                    _logger.LogError("Todo object sent from client is null.");
                    return BadRequest("Todo object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid todo object sent from client.");
                    return BadRequest("Invalid model object");
                }
                todoModel.Completed = false;
                todoModel.CreationDate = DateTime.Now;

                var todoEntity = _mapper.Map<TodoDTO>(todoModel);

                todoService.AddTodo(todoEntity);

                return CreatedAtAction("GetTodoById", new { id = todoEntity.TodoId }, todoEntity);
                //return Ok(todoModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostTodo action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, [FromBody]TodoViewModel todoModel)
        {
            try
            {
                if (todoModel == null)
                {
                    _logger.LogError("Todo object sent from client is null.");
                    return BadRequest("Todo object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid todo object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var todoEntity = await todoService.GetTodo(id);
                if (todoEntity == null)
                {
                    _logger.LogError($"Todo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                todoService.EditTodo(_mapper.Map<TodoDTO>(todoModel));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutTodo action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoViewModel>> DeleteTodo(int id)
        {
            try
            {
                var todo = await todoService.GetTodo(id);
                if (todo == null)
                {
                    _logger.LogError($"Todo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                todoService.DeleteTodo(todo);

                //return NoContent();
                return _mapper.Map<TodoViewModel>(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTodo action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[Route("/deleteall")]
        [HttpDelete]
        public IActionResult DeleteAllTodos()
        {
            try
            {
                todoService.DeleteAllTodos();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAllTodos action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}