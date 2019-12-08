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
    public class TodoAdapter : ITodoAdapter
    {
        private readonly IMapper mapper;
        private readonly ITodoService todoService;

        public TodoAdapter(IMapper mapper, ITodoService todoService)
        {
            this.mapper = mapper;
            this.todoService = todoService;
        }

        public async Task<List<TodoViewModel>> GetTodos()
        {
            return mapper.Map<List<TodoViewModel>>(await todoService.GetAllAsync<Todo>());
        }

        public async Task<TodoViewModel> GetTodo(int id)
        {
            return mapper.Map<TodoViewModel>(await todoService.GetAsync(x => x.TodoId == id));
        }

        public async Task AddTodo(TodoViewModel todoViewModel)
        {
            await todoService.CreateAsync(mapper.Map<Todo>(todoViewModel));
        }

        public async Task UpdateTodo(TodoViewModel todoViewModel)
        {
            var updatedTodo = mapper.Map<Todo>(todoViewModel);
            await todoService.UpdateAsync(updatedTodo);
        }

        public async Task DeleteTodo(int id)
        {
            var todo = await todoService.GetAsync<Todo>(x => x.TodoId == id);
            await todoService.DeleteAsync(todo);
        }

        public async Task DeleteAllTodos()
        {
            await todoService.DeleteAllAsync<Todo>();
        }
    }
}
