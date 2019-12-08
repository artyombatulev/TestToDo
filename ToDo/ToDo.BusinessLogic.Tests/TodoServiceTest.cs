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
    public class TodoServiceTest
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

        private List<Todo> GetTestTodos()
        {
            return new List<Todo>
            {
                new Todo
                {
                    TodoId = 1,
                    Title = "test todo",
                    Points = new List<TodoPoint>{
                        new TodoPoint
                        {
                            PointId = 1,
                            TodoId = 1,
                            Description = "test point"
                        }
                    }
                },
                new Todo
                {
                    TodoId = 2,
                    Title = "test todo 2",
                    Points = new List<TodoPoint>{}
                }
            };
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllTodos()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodos = GetTestTodos();

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);

            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            var todos = await todoService.GetAllAsync<Todo>();

            //Assert  
            Assert.IsAssignableFrom<IEnumerable<Todo>>(todos);
            Assert.Equal(2, todos.Count);
        }

        [Fact]
        public async Task GetAsync_Should_ReturnTodo()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoId = 1;

            var testTodos = GetTestTodos();

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);

            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            var todo = await todoService.GetAsync<Todo>(x => x.TodoId == testTodoId);

            //Assert  
            Assert.IsAssignableFrom<Todo>(todo);
        }

        [Fact]
        public async Task DeleteTodo_Should_DeleteTodo()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodoId = 1;

            var testTodos = GetTestTodos();

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);
            todoDbSetMock.Setup(x => x.Remove(testTodos.FirstOrDefault()))
                .Callback(() => testTodos.Clear());

            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            var todoBeforeDelete = await todoService.GetAsync<Todo>(x => x.TodoId == testTodoId);
            await todoService.DeleteAsync(todoBeforeDelete);
            var todoAfterDelete = await todoService.GetAsync<Todo>(x => x.TodoId == testTodoId);

            //Assert  
            Assert.NotNull(todoBeforeDelete);
            Assert.Null(todoAfterDelete);
            Assert.IsAssignableFrom<IEnumerable<Todo>>(testTodos);
            Assert.NotNull(testTodos);
        }

        [Fact]
        public async Task DeleteAllAsync_Should_DeleteAllTodos()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodos = GetTestTodos();

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);

            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            var todosBefore = await todoService.GetAllAsync<Todo>();
            await todoService.DeleteAllAsync<Todo>();
            var todosAfter = await todoService.GetAllAsync<Todo>();

            //Assert  
            Assert.IsAssignableFrom<IEnumerable<Todo>>(todosBefore);
            Assert.NotNull(todosBefore);
            Assert.Empty(todosAfter);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateTodo()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodo = new Todo
            {
                TodoId = 2,
                Title = "updated",
                Points = new List<TodoPoint> { }
            };

            var testTodos = GetTestTodos();

            var todoBefore = testTodos.Where(x => x.TodoId == testTodo.TodoId).FirstOrDefault();
            string todoTitle = todoBefore.Title;

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);
            todoDbSetMock.Setup(x => x.Update(It.IsAny<Todo>()))
                .Callback(() =>
                {
                    var todo = testTodos.Where(x => x.TodoId == testTodo.TodoId).FirstOrDefault();
                    todo.Title = testTodo.Title;
                });
            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            await todoService.UpdateAsync(testTodo);
            var todoAfter = await todoService.GetAsync<Todo>(x => x.TodoId == testTodo.TodoId);

            //Assert  
            Assert.NotEqual(todoTitle, todoBefore.Title);
            Assert.IsAssignableFrom<Todo>(todoAfter);
            Assert.Equal(testTodo.Title, todoAfter.Title);
        }

        [Fact]
        public async Task CreateAsync_Should_CreateTodo()
        {
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<ITodoContext>();

            var testTodo = new Todo
            {
                TodoId = 3,
                Title = "test todo 3",
                Points = new List<TodoPoint> { }
            };

            var testTodos = GetTestTodos();

            var todoDbSetMock = testTodos.AsQueryable().GetMockDbSet();

            dbContextMock.Setup(x => x.DbSet<Todo>()).Returns(todoDbSetMock.Object);
            todoDbSetMock.Setup(x => x.Add(It.IsAny<Todo>()))
                .Callback(() =>
                {
                    testTodos.Add(testTodo);
                });
            //Execute
            var todoService = new TodoService(dbContextMock.Object);
            await todoService.CreateAsync(testTodo);
            var todo = await todoService.GetAsync<Todo>(x => x.TodoId == testTodo.TodoId);

            //Assert  
            Assert.IsAssignableFrom<Todo>(todo);
            Assert.Equal(testTodo.Title, todo.Title);
            Assert.Equal(3, testTodos.Count);
        }

    }
}
