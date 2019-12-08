using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ToDo.PresentationAdapters;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
    [Route("api/point")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly ILogger<PointController> logger;
        private readonly ITodoPointAdapter todoPointAdapter;

        public PointController(ITodoPointAdapter todoPointAdapter, ILogger<PointController> logger)
        {
            this.todoPointAdapter = todoPointAdapter;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> PostPoint([FromBody]TodoPointViewModel pointModel)
        {
            try
            {
                if (pointModel == null)
                {
                    logger.LogError("Point object sent from client is null.");
                    return BadRequest("Point object is null");
                }
                await todoPointAdapter.AddPoint(pointModel);

                return Ok(pointModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PostPoint action: {ex.Message}");
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
                    logger.LogError("Point object sent from client is null.");
                    return BadRequest("Point object is null");
                }

                if (pointModel.IsCompleted == true && pointModel.DateOfComplition == null)
                {
                    pointModel.DateOfComplition = DateTime.UtcNow;
                }

                if (pointModel.IsCompleted == false && pointModel.DateOfComplition != null)
                {
                    pointModel.DateOfComplition = null;
                }

                await todoPointAdapter.UpdatePoint(pointModel);

                return Ok(pointModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PutPoint action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<TodoPointViewModel>> DeletePoint(int id, [FromBody]TodoPointViewModel pointModel)
        {
            try
            {
                await todoPointAdapter.DeletePoint(pointModel.PointId);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeletePoint action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllPoints(int id)
        {
            try
            {
                await todoPointAdapter.DeletePointsByTodoId(id);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAllPoints action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}