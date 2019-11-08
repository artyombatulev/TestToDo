using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
//using System.Threading.Tasks;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private TodoContext db;
        private TodoRepository todoRepository;
        private TodoPointRepository todoPointRepository;

        public EFUnitOfWork(TodoContext todoContext)
        {
            db = todoContext;
        }
        public IRepository<Todo> Todos
        {
            get
            {
                if (todoRepository == null)
                    todoRepository = new TodoRepository(db);
                return todoRepository;
            }
        }

        public IRepository<TodoPoint> TodoPoints
        {
            get
            {
                if (todoPointRepository == null)
                    todoPointRepository = new TodoPointRepository(db);
                return todoPointRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
        /*public async Task Save()
        {
            await db.SaveChangesAsync();
        }*/

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
