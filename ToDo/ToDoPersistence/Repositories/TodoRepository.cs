using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(TodoContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
