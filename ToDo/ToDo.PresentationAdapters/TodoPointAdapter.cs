using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.BusinessLogic.Interfaces;
using ToDo.DataAccessModels;
using ToDo.ViewModels;

namespace ToDo.PresentationAdapters
{
    public class TodoPointAdapter : ITodoPointAdapter
    {
        private readonly IMapper mapper;
        private readonly ITodoPointService todoPointService;

        public TodoPointAdapter(IMapper mapper, ITodoPointService todoService)
        {
            this.mapper = mapper;
            this.todoPointService = todoService;
        }

        public async Task<List<TodoPointViewModel>> GetPoints()
        {
            return mapper.Map<List<TodoPointViewModel>>(await todoPointService.GetAllAsync<TodoPoint>());
        }

        public async Task<TodoPointViewModel> GetPoint(int id)
        {
            return mapper.Map<TodoPointViewModel>(await todoPointService.GetAsync<TodoPoint>(x => x.PointId == id));
        }

        public async Task AddPoint(TodoPointViewModel todoPointViewModel)
        {
            await todoPointService.CreateAsync(mapper.Map<TodoPoint>(todoPointViewModel));
        }

        public async Task UpdatePoint(TodoPointViewModel todoPointViewModel)
        {
            var updatedTodo = mapper.Map<TodoPoint>(todoPointViewModel);
            await todoPointService.UpdateAsync(updatedTodo);
        }

        public async Task DeletePoint(int id)
        {
            var todo = await todoPointService.GetAsync<TodoPoint>(x => x.PointId == id);
            await todoPointService.DeleteAsync(todo);
        }

        public async Task DeleteAllPoints()
        {
            await todoPointService.DeleteAllAsync<TodoPoint>();
        }

        public async Task DeletePointsByTodoId(int id)
        {
            await todoPointService.DeletePointsByTodoId(id);
        }
    }
}
