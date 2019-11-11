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
        private readonly IMapper _mapper;
        ITodoService todoService;
        ITodoPointService pointService;

        public PointController(ITodoService todoserv,ITodoPointService pointserv, ILogger<PointController> logger, IMapper mapper)
        {
            todoService = todoserv;
            pointService = pointserv;
            _logger = logger;
            _mapper = mapper;
        }

       // [Route("api/point/todo/")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> GetAllPointsByTodoId(int id)
        {
            try
            {
                var todo = await todoService.GetTodoWithPoints(id);
                if (todo == null)
                {
                    _logger.LogError($"Todo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned todo with id: {id}");
                    return _mapper.Map<TodoViewModel>(todo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllPointsByTodoId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet("{id}")]
        public async Task<ActionResult<TodoPointViewModel>> GetPointById(int id)
        {

            try
            {
                var point = await pointService.GetPoint(id);
                if (point == null)
                {
                    _logger.LogError($"Point with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned point with id: {id}");

                    var pointResult = _mapper.Map<TodoPointViewModel>(point);
                    return pointResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPointById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult<TodoPointViewModel> PostPoint([FromBody]TodoPointViewModel pointModel)
        {
            try
            {
                if (pointModel == null)
                {
                    _logger.LogError("Point object sent from client is null.");
                    return BadRequest("Point object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid point object sent from client.");
                    return BadRequest("Invalid model object");
                }
                pointModel.IsCompleted = false;

                var pointEntity = _mapper.Map<TodoPointDTO>(pointModel);

                pointService.AddPoint(pointEntity);

                return CreatedAtAction("GetPointById", new { id = pointEntity.PointId }, pointEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostPoint action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoint(int id, [FromBody]TodoPointViewModel pointModel)
        {
            try
            {
                if (pointModel == null)
                {
                    _logger.LogError("Point object sent from client is null.");
                    return BadRequest("Point object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid point object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var pointEntity = await pointService.GetPoint(id);
                if (pointEntity == null)
                {
                    _logger.LogError($"Point with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (pointModel.IsCompleted == true && pointModel.DateOfComplition == null)
                {
                    pointModel.DateOfComplition = DateTime.Now;
                }
                if (pointModel.IsCompleted == false && pointModel.DateOfComplition != null)
                {
                    pointModel.DateOfComplition = null;
                }

                pointService.EditPoint(_mapper.Map<TodoPointDTO>(pointModel));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutPoint action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<TodoPointViewModel>> DeletePoint(int id,[FromBody]TodoPointViewModel pointModel)
        {
            try
            {
                var point = await pointService.GetPoint(pointModel.PointId);
                if (point == null)
                {
                    _logger.LogError($"Point with id: {pointModel.PointId}, hasn't been found in db.");
                    return NotFound();
                }

                pointService.DeletePoint(point);

                //return NoContent();
                return _mapper.Map<TodoPointViewModel>(point);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeletePoint action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
       [HttpDelete("{id}")]
        public IActionResult DeleteAllPoints(int id)
        {
            try
            {
                pointService.DeletePointsByTodoId(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAllPoints action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}