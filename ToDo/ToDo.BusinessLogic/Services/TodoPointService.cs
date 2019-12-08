using System.Threading.Tasks;
using ToDo.DataAccessModels;
using ToDo.BusinessLogic.Base;
using ToDo.BusinessLogic.Interfaces;
using ToDo.Persistence.DatabaseContext;

namespace ToDo.BusinessLogic.Services
{
    public class TodoPointService : BaseService, ITodoPointService
    {
        public TodoPointService(ITodoContext dbContext) : base(dbContext)
        {
        }

        public async Task DeletePointsByTodoId(int id)
        {
            var todo = await GetAsync<Todo>(x => x.TodoId == id);
            await DeleteAllAsync(todo.Points);
        }
    }
}
