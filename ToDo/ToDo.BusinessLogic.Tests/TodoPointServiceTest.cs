using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.BusinessLogic.Services;
using ToDo.DataAccessModels;
using ToDo.Persistence.DatabaseContext;
using ToDo.Tests.Core;
using Xunit;

namespace ToDo.BusinessLogic.Tests
{
    public class TodoPointServiceTest
    {
        private List<Todo> GetTestTodo(int id, List<TodoPoint> points)
        {
            return new List<Todo> {
                new Todo
                {
                    TodoId = id,
                    Title = "test todo",
                    Points = points
                }
            };
        }

        private List<Todo> GetTestTodos(List<TodoPoint> points)
        {
            return new List<Todo> {
                new Todo
                {
                    TodoId = 1,
                    Title = "test todo 1",
                    Points = points
                },
                new Todo
                {
                    TodoId = 2,
                    Title = "test todo 2",
                    Points = new List<TodoPoint>{}
                }
            };
        }

        private List<TodoPoint> GetTestTodoPoint(int id, int todoId)
        {
            return new List<TodoPoint>
            {
                new TodoPoint
                {
                    PointId = id,
                    TodoId = todoId,
                    Description = "test point"
                }
            };
        }

        private List<TodoPoint> GetTestTodoPoints()
        {
            return new List<TodoPoint>
            {
                new TodoPoint
                {
                    PointId = 1,
                    TodoId = 1,
                    Description = "test point"
                },
                new TodoPoint
                {
                    PointId = 2,
                    TodoId = 1,
                    Description = "test point 2"
                }
            };
        }

        [Fact]
        public async Task DeletePointsByTodoId_Should_DeletePoints()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoId = 1;
            var testTodoPointId = 1;

            var testPoints = GetTestTodoPoint(testTodoPointId, testTodoId);
            var testTodos = GetTestTodo(testTodoId, testPoints);

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();
            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);
            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);
            todoPointDbSetMock.Setup(x => x.RemoveRange(It.IsAny<List<TodoPoint>>()))
                .Callback(() => testPoints.Clear());

            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            var todoPointBeforeDelete = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPointId);
            await todoPointService.DeletePointsByTodoId(testTodoId);

            //Assert  
            Assert.NotNull(todoPointBeforeDelete);
            Assert.Empty(testPoints);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllPoints()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testPoints = GetTestTodoPoints();

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);

            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            var todoPoints = await todoPointService.GetAllAsync<TodoPoint>();

            //Assert  
            Assert.IsAssignableFrom<IEnumerable<TodoPoint>>(todoPoints);
            Assert.Equal(2, todoPoints.Count);
        }

        [Fact]
        public async Task GetAsync_Should_ReturnPoint()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoPointId = 1;

            var testPoints = GetTestTodoPoints();

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);

            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            var todoPoint = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPointId);

            //Assert  
            Assert.IsAssignableFrom<TodoPoint>(todoPoint);
        }

        [Fact]
        public async Task DeletePoint_Should_DeletePoint()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoPointId = 1;

            var testPoints = GetTestTodoPoints();

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);
            todoPointDbSetMock.Setup(x => x.Remove(testPoints.FirstOrDefault()))
                .Callback(() => testPoints.Clear());

            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            var todoPointBeforeDelete = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPointId);
            await todoPointService.DeleteAsync(todoPointBeforeDelete);
            var todoPointAfterDelete = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPointId);

            //Assert  
            Assert.NotNull(todoPointBeforeDelete);
            Assert.Null(todoPointAfterDelete);
            Assert.IsAssignableFrom<IEnumerable<TodoPoint>>(testPoints);
            Assert.NotNull(testPoints);
        }

        [Fact]
        public async Task DeleteAllAsync_Should_DeleteAllPoints()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testPoints = GetTestTodoPoints();

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);

            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            var todoPointsBefore = await todoPointService.GetAllAsync<TodoPoint>();
            await todoPointService.DeleteAllAsync<TodoPoint>();
            var todoPointsAfter = await todoPointService.GetAllAsync<TodoPoint>();

            //Assert  
            Assert.IsAssignableFrom<IEnumerable<TodoPoint>>(todoPointsBefore);
            Assert.NotNull(todoPointsBefore);
            Assert.Empty(todoPointsAfter);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdatePoint()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoPoint = new TodoPoint
            {
                PointId = 2,
                TodoId = 1,
                Description = "updated"
            };

            var testPoints = GetTestTodoPoints();

            var pointBefore = testPoints.Where(x => x.PointId == testTodoPoint.PointId).FirstOrDefault();
            string pointDescription = pointBefore.Description;

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);
            todoPointDbSetMock.Setup(x => x.Update(It.IsAny<TodoPoint>()))
                .Callback(() =>
                {
                    var point = testPoints.Where(x => x.PointId == testTodoPoint.PointId).FirstOrDefault();
                    point.Description = testTodoPoint.Description;
                });
            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            await todoPointService.UpdateAsync(testTodoPoint);
            var todoPointAfter = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPoint.PointId);

            //Assert  
            Assert.NotEqual(pointDescription, pointBefore.Description);
            Assert.IsAssignableFrom<TodoPoint>(todoPointAfter);
            Assert.Equal(testTodoPoint.Description, todoPointAfter.Description);
        }

        [Fact]
        public async Task CreateAsync_Should_CreatePoint()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoPoint = new TodoPoint
            {
                PointId = 3,
                TodoId = 2,
                Description = "test point 3"
            };

            var testPoints = GetTestTodoPoints();

            var todoPointDbSetMock = testPoints.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<TodoPoint>()).Returns(todoPointDbSetMock.Object);
            todoPointDbSetMock.Setup(x => x.Add(It.IsAny<TodoPoint>()))
                .Callback(() =>
                {
                    testPoints.Add(testTodoPoint);
                });
            //Execute
            var todoPointService = new TodoPointService(dbContextMock.Object);
            await todoPointService.CreateAsync(testTodoPoint);
            var todoPoint = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == testTodoPoint.PointId);

            //Assert  
            Assert.IsAssignableFrom<TodoPoint>(todoPoint);
            Assert.Equal(testTodoPoint.Description, todoPoint.Description);
            Assert.Equal(3, testPoints.Count);
        }
    }
}
