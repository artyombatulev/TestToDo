using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo.Models;
using ToDoBusinessLogic.DTO;
using ToDoBusinessLogic.Interfaces;

namespace ToDo.Controllers
{
    [Route("api/point")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly ILogger<PointController> _logger;
        ITodoService todoService;
        ITodoPointService pointService;

        public PointController(ITodoService todoserv,ITodoPointService pointserv, ILogger<PointController> logger)
        {
            todoService = todoserv;
            pointService = pointserv;
            _logger = logger;
        }

       // [Route("api/point/todo/")]
        [HttpGet("{id}")]
        public IEnumerable<TodoPointViewModel> GetAllPointsByTodoId(int id)
        {
            /*
            IEnumerable<TodoPointDTO> pointDtos = pointService.GetTodoPoints();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoPointDTO, TodoPointViewModel>()).CreateMapper();
            return mapper.Map<IEnumerable<TodoPointDTO>, List<TodoPointViewModel>>(pointDtos.Where(x => x.TodoId == id));
        */

            IEnumerable<TodoPointDTO> pointDtos = pointService.GetTodoPoints();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoPointDTO, TodoPointViewModel>()).CreateMapper();
            var points = mapper.Map<IEnumerable<TodoPointDTO>, List<TodoPointViewModel>>(pointDtos.Where(x => x.TodoId == id));
            /*
            int countPoints = points.Count;
            int count = 0;
            foreach (TodoPointViewModel pvm in points)
            {
                if (pvm.IsCompleted == true)
                    count++;
            }
            if (count == countPoints)
            {
                TodoDTO todoDto = todoService.GetTodo(id);
                todoDto.Completed = true;
                todoService.EditTodo(todoDto);
            }
            else
            {
                TodoDTO todoDto = todoService.GetTodo(id);
                todoDto.Completed = false;
                todoService.EditTodo(todoDto);
            }
            */
            return points;
        }

        [HttpPost]
        public IActionResult Post([FromBody]TodoPointViewModel pointModel)
        {
            if (ModelState.IsValid)
            {
                var pointDto = new TodoPointDTO { PointId = pointModel.PointId, Description = pointModel.Description, IsCompleted = pointModel.IsCompleted, DateOfComplition = pointModel.DateOfComplition, TodoId = pointModel.TodoId };

                pointService.AddPoint(pointDto);

                return Ok(pointModel);


            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TodoPointViewModel pointModel)
        {
            if (ModelState.IsValid)
            {
                if(pointModel.IsCompleted == true && pointModel.DateOfComplition == null)
                {
                    pointModel.DateOfComplition = DateTime.Now;
                }
                if(pointModel.IsCompleted == false && pointModel.DateOfComplition != null)
                {
                    pointModel.DateOfComplition = null;
                }
                var pointDto = new TodoPointDTO { PointId = pointModel.PointId, Description = pointModel.Description, IsCompleted = pointModel.IsCompleted, DateOfComplition = pointModel.DateOfComplition, TodoId = pointModel.TodoId };
                pointService.EditPoint(pointDto);
                return Ok(pointModel);
            }
            return BadRequest(ModelState);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            TodoPointDTO pointDto = pointService.GetTodoPoint(id);
            if (pointDto != null)
            {
                pointService.DeletePoint(id);

            }
            return Ok(pointDto);
        }
        [HttpDelete]
        public void DeleteAllPoints(int[] id)
        {

            foreach (int i in id)
            {
                pointService.DeletePoint(i);
            }
            /*
            TodoDTO todoDto = todoService.GetTodo(id);
            if (todoDto != null)
            {
                todoService.DeleteTodo(id);

            }
            return Ok(todoDto);
            
            IEnumerable<TodoPointDTO> pointDtos = pointService.GetTodoPoints();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoPointDTO, TodoPointViewModel>()).CreateMapper();
            var points = mapper.Map<IEnumerable<TodoPointDTO>, List<TodoPointViewModel>>(pointDtos.Where(x => x.TodoId == id));
            foreach(TodoPointViewModel pvm in points)
            {
                pointService.DeletePoint(pvm.PointId);
            }
            return Ok(points);*/
            //pointService.DeletePointsByTodoId(id);
        }
    }
}