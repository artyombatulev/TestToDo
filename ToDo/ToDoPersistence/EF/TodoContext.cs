using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoPersistence.Entities;

namespace ToDoPersistence.EF
{
    public class TodoContext : DbContext
    {

        public TodoContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoPoint> TodoPoints { get; set; }
    }
}
