using System;
using System.Collections.Generic;
using System.Text;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public class TodoPointRepository : Repository<TodoPoint>, ITodoPointRepository
    {
        public TodoPointRepository(TodoContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
