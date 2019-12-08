using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Controllers;
using ToDo.PresentationAdapters;
using ToDo.ViewModels;
using Xunit;

namespace ToDo.Controller.Tests
{
    public class PointControllerTest
    {
        private List<TodoViewModel> GetTestTodos(List<TodoPointViewModel> points)
        {
            return new List<TodoViewModel>
            {
                new TodoViewModel
                {
                    TodoId = 1,
                    Title = "test todo",
                    Points = points
                },
                new TodoViewModel
                {
                    TodoId = 2,
                    Title = "test todo 2",
                    Points = new List<TodoPointViewModel>{}
                }
            };
        }

        private List<TodoPointViewModel> GetTestPoints()
        {
            return new List<TodoPointViewModel>
            {
                new TodoPointViewModel
                {
                    PointId = 1,
                    TodoId = 1,
                    Description = "test point"
                },
                new TodoPointViewModel
                {
                    PointId = 2,
                    TodoId = 1,
                    Description = "test point 2"
                }
            };
        }

        // PostPoint
        [Fact]
        public async Task PostPoint_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var testPoint = new TodoPointViewModel
            {
                PointId = 3,
                TodoId = 2,
                Description = "test point 3"
            };

            adapterMock.Setup(x => x.AddPoint(testPoint)).Returns(Task.FromResult(testPoint));
            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PostPoint(testPoint);

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<TodoPointViewModel>(okResult.Value);
            Assert.Equal(testPoint.Description, returnValue.Description);
        }

        [Fact]
        public async Task PostPoint_Should_ReturnBadRequest()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PostPoint(null);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
            Assert.Equal("Point object is null", returnValue.Value);
        }

        // PutPoint
        [Fact]
        public async Task PutPoint_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var testPoint = new TodoPointViewModel
            {
                PointId = 2,
                TodoId = 1,
                Description = "Updated"
            };
            var testPoints = GetTestPoints();

            adapterMock.Setup(x => x.UpdatePoint(It.IsAny<TodoPointViewModel>()))
                .Callback(() =>
                {
                    var point = testPoints.Where(x => x.PointId == testPoint.PointId).FirstOrDefault();
                    point.Description = testPoint.Description;
                });

            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PutPoint(testPoint.PointId, testPoint);
            var pointAfterUpdate = testPoints.Where(x => x.PointId == testPoint.PointId).FirstOrDefault();

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<TodoPointViewModel>(okResult.Value);
            Assert.Equal(testPoint.Description, pointAfterUpdate.Description);
            Assert.Equal(2, testPoints.Count);
        }

        [Fact]
        public async Task PutPoint_Should_ReturnBadRequest()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PutPoint(5, null);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
            Assert.Equal("Point object is null", returnValue.Value);
        }

        // DeletePoint
        [Fact]
        public async Task DeletePoint_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var testPoint = new TodoPointViewModel
            {
                PointId = 2,
                TodoId = 1,
                Description = "test point 2"
            };
            var testPoints = GetTestPoints();

            adapterMock.Setup(x => x.DeletePoint(It.IsAny<int>()))
                .Callback(() => testPoints.Remove(testPoints.Where(x => x.PointId == testPoint.PointId).FirstOrDefault()));

            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.DeletePoint(testPoint.PointId, testPoint);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<TodoPointViewModel>>(result);
            var returnValue = Assert.IsAssignableFrom<OkResult>(actionResult.Result);
            Assert.Single(testPoints);
        }

        // DeleteAllPoints
        [Fact]
        public async Task DeleteAllPoints_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoPointAdapter>();
            var loggerMock = new Mock<ILogger<PointController>>();

            var todoIdForDeletePoints = 1;
            var testPoints = GetTestPoints();
            var testTodos = GetTestTodos(testPoints);
            int countPointsBeforeDelete = testPoints.Count;

            adapterMock.Setup(x => x.DeletePointsByTodoId(todoIdForDeletePoints))
                .Callback(() => testPoints.Clear());

            var controller = new PointController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.DeleteAllPoints(todoIdForDeletePoints);
            var todo = testTodos.Where(x => x.TodoId == todoIdForDeletePoints).FirstOrDefault();

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsAssignableFrom<OkResult>(actionResult);
            Assert.Equal(2, countPointsBeforeDelete);
            Assert.Empty(todo.Points);
        }

    }
}
