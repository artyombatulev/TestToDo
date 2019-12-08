using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.BusinessLogic.Base;
using ToDo.BusinessLogic.Interfaces;
using ToDo.DataAccessModels;
using ToDo.Persistence.DatabaseContext;

namespace ToDo.BusinessLogic.Services
{
    public class TodoService : BaseService, ITodoService
    {
        public TodoService(ITodoContext dbContext) : base(dbContext)
        {
        }

        public async Task<Todo> GetAsync(Expression<Func<Todo, bool>> predicate) 
        {
            var todo = await dbContext.DbSet<Todo>().FirstOrDefaultAsync(predicate);
            int countPoints = todo.Points.Count;
            int countCompletedPoints = todo.Points.Count(x => x.IsCompleted == true);

            if (countCompletedPoints == countPoints && countPoints != 0)
            {
                if (todo.Completed != true)
                {
                    todo.Completed = true;
                    dbContext.DbSet<Todo>().Update(todo);
                    await dbContext.SaveAsync();
                }
            }
            else
            {
                if (todo.Completed != false)
                {
                    todo.Completed = false;
                    dbContext.DbSet<Todo>().Update(todo);
                    await dbContext.SaveAsync();
                }
            }

            return todo;
        }
    }
}
