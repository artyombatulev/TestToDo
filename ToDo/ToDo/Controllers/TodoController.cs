using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.PresentationAdapters;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> logger;
        private readonly ITodoAdapter todoAdapter;

        public TodoController(ITodoAdapter todoAdapter, ILogger<TodoController> logger)
        {
            this.todoAdapter = todoAdapter;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoViewModel>> GetAllTodos()
        {
            var todos = await todoAdapter.GetTodos();

            logger.LogInformation($"Returned all todos from database.");

            return todos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> GetTodoById(int id)
        {
            try
            {
                var todo = await todoAdapter.GetTodo(id);

                if (todo == null)
                {
                    logger.LogError($"Todo with id: {id}, hasn't been found in db.");
                    return NotFound(id);
                }

                logger.LogInformation($"Returned todo with id: {id}");

                return todo;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetTodoById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostTodo([FromBody]TodoViewModel todoModel)
        {
            try
            {
                if (todoModel == null)
                {
                    logger.LogError("Todo object sent from client is null.");
                    return BadRequest("Todo object is null");
                }

                todoModel.CreationDate = DateTime.UtcNow;

                await todoAdapter.AddTodo(todoModel);

                return Ok(todoModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PostTodo action: {ex.Message}");
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
                    logger.LogError("Todo object sent from client is null.");
                    return BadRequest("Todo object is null");
                }

                await todoAdapter.UpdateTodo(todoModel);

                return Ok(todoModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PutTodo action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoViewModel>> DeleteTodo(int id)
        {
            try
            {
                await todoAdapter.DeleteTodo(id);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteTodo action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllTodos()
        {
            try
            {
                await todoAdapter.DeleteAllTodos();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAllTodos action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}