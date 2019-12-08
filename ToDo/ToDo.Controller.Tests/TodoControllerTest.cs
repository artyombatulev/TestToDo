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
    public class TodoControllerTest
    {
        private List<TodoViewModel> GetTestTodos()
        {
            return new List<TodoViewModel>
            {
                new TodoViewModel
                {
                    TodoId = 1,
                    Title = "test todo",
                    Points = new List<TodoPointViewModel>{
                        new TodoPointViewModel
                        {
                            PointId = 1,
                            TodoId = 1,
                            Description = "test point"
                        }
                    }
                },
                new TodoViewModel
                {
                    TodoId = 2,
                    Title = "test todo 2",
                    Points = new List<TodoPointViewModel>{}
                }
            };
        }

        // GetAllTodos
        [Fact]
        public async Task GetAllTodos_Should_ReturnAllTodos()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            adapterMock.Setup(x => x.GetTodos()).Returns(Task.FromResult(GetTestTodos()));
            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetAllTodos();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<TodoViewModel>>(result);
            Assert.Equal(2, result.Count());
        }

        // GetTodoById
        [Fact]
        public async Task GetTodoById_Should_ReturnTodoWithId()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodoId = 1;

            adapterMock.Setup(x => x.GetTodo(testTodoId)).Returns(Task.FromResult(GetTestTodos().FirstOrDefault(s => s.TodoId == testTodoId)));
            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetTodoById(testTodoId);

            // Assert
            Assert.IsAssignableFrom<ActionResult<TodoViewModel>>(result);
            Assert.Equal(result.Value.TodoId, testTodoId);
        }

        [Fact]
        public async Task GetTodoById_Should_ReturnNotFound()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodoId = 123;

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetTodoById(testTodoId);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<TodoViewModel>>(result);
            Assert.IsAssignableFrom<NotFoundObjectResult>(actionResult.Result);
        }

        // PostTodo
        [Fact]
        public async Task PostTodo_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodo = new TodoViewModel
            {
                TodoId = 3,
                Title = "test todo 3",
                Points = new List<TodoPointViewModel> { }
            };

            adapterMock.Setup(x => x.AddTodo(testTodo)).Returns(Task.FromResult(testTodo));
            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PostTodo(testTodo);

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<TodoViewModel>(okResult.Value);
            Assert.Equal(testTodo.Title, returnValue.Title);
        }

        [Fact]
        public async Task PostTodo_Should_ReturnBadRequest()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PostTodo(null);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
            Assert.Equal("Todo object is null", returnValue.Value);
        }

        // PutTodo
        [Fact]
        public async Task PutTodo_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodo = new TodoViewModel
            {
                TodoId = 2,
                Title = "Updated",
                Points = new List<TodoPointViewModel> { }
            };
            var testTodos = GetTestTodos();

            adapterMock.Setup(x => x.UpdateTodo(It.IsAny<TodoViewModel>()))
                .Callback(() =>
                {
                    var todo = testTodos.Where(x => x.TodoId == testTodo.TodoId).FirstOrDefault();
                    todo.Title = testTodo.Title;
                });

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PutTodo(testTodo.TodoId, testTodo);
            var todoAfterUpdate = testTodos.Where(x => x.TodoId == testTodo.TodoId).FirstOrDefault();

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<TodoViewModel>(okResult.Value);
            Assert.Equal(testTodo.Title, todoAfterUpdate.Title);
            Assert.Equal(2, testTodos.Count);
        }

        [Fact]
        public async Task PutTodo_Should_ReturnBadRequest()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.PutTodo(5, null);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
            Assert.Equal("Todo object is null", returnValue.Value);
        }

        // DeleteTodo
        [Fact]
        public async Task DeleteTodo_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodoId = 2;
            var testTodos = GetTestTodos();

            adapterMock.Setup(x => x.DeleteTodo(It.IsAny<int>()))
                .Callback(() => testTodos.Remove(testTodos.Where(x => x.TodoId == testTodoId).FirstOrDefault()));

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.DeleteTodo(testTodoId);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<TodoViewModel>>(result);
            var returnValue = Assert.IsAssignableFrom<OkResult>(actionResult.Result);
            Assert.Single(testTodos);
        }

        // DeleteAllTodos
        [Fact]
        public async Task DeleteAllTodos_Should_ReturnOk()
        {
            // Arrange
            var adapterMock = new Mock<ITodoAdapter>();
            var loggerMock = new Mock<ILogger<TodoController>>();

            var testTodos = GetTestTodos();
            int countTodosBeforeDelete = testTodos.Count;

            adapterMock.Setup(x => x.DeleteAllTodos())
                .Callback(() => testTodos.Clear());

            var controller = new TodoController(adapterMock.Object, loggerMock.Object);

            // Act
            var result = await controller.DeleteAllTodos();

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsAssignableFrom<OkResult>(actionResult);
            Assert.Equal(2, countTodosBeforeDelete);
            Assert.Empty(testTodos);
        }
    }
}
