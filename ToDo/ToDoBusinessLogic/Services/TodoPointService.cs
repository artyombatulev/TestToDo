using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoBusinessLogic.DTO;
using ToDoBusinessLogic.Infrastructure;
using ToDoBusinessLogic.Interfaces;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoBusinessLogic.Services
{
    public class TodoPointService : ITodoPointService
    {
        IUnitOfWork Database { get; set; }

        public TodoPointService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<TodoPointDTO> GetTodoPoints()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoPoint, TodoPointDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<TodoPoint>, List<TodoPointDTO>>(Database.TodoPoints.GetAll());
        }

        public TodoPointDTO GetTodoPoint(int? id)
        {
            if (id == null)
                throw new ValidationException("Not found Point Id", "");
            var todoPoint = Database.TodoPoints.Get(id.Value);
            if (todoPoint == null)
                throw new ValidationException("Point not found", "");

            return new TodoPointDTO { PointId = todoPoint.PointId, Description = todoPoint.Description, IsCompleted = todoPoint.IsCompleted, DateOfComplition = todoPoint.DateOfComplition, TodoId = todoPoint.TodoId };
        }

        public void AddPoint(TodoPointDTO pointDto)
        {
            var point = new TodoPoint { PointId = pointDto.PointId, Description = pointDto.Description, DateOfComplition = pointDto.DateOfComplition, IsCompleted = pointDto.IsCompleted, TodoId = pointDto.TodoId };
            Database.TodoPoints.Create(point);
            Database.Save();
        }

        public void EditPoint(TodoPointDTO pointDto)
        {
            var point = new TodoPoint { PointId = pointDto.PointId, Description = pointDto.Description, DateOfComplition = pointDto.DateOfComplition, IsCompleted = pointDto.IsCompleted, TodoId = pointDto.TodoId };
            Database.TodoPoints.Update(point);
            Database.Save();

        }

        public void DeletePoint(int? id)
        {
            var point = Database.TodoPoints.Get(id.Value);
            Database.TodoPoints.Delete(point);
            Database.Save();
        }

        public void DeletePointsByTodoId(int? id)
        {
            var pointsall = Database.TodoPoints.GetAll();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TodoPoint, TodoPointDTO>()).CreateMapper();
            var points = mapper.Map<IEnumerable<TodoPoint>, List<TodoPointDTO>>(pointsall.Where(x => x.TodoId == id));
            foreach(TodoPointDTO point in points)
            {
                DeletePoint(point.PointId);
            }
            Database.Save();

        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
