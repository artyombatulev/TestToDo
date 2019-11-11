using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using System.Threading.Tasks;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private TodoContext db;
        private ITodoRepository _todoRepository;
        private ITodoPointRepository _todoPointRepository;

        public EFUnitOfWork(TodoContext todoContext)
        {
            db = todoContext;
        }
        public ITodoRepository TodoRep
        {
            get
            {
                if (_todoRepository == null)
                    _todoRepository = new TodoRepository(db);
                return _todoRepository;
            }
        }

        public ITodoPointRepository TodoPointRep
        {
            get
            {
                if (_todoPointRepository == null)
                    _todoPointRepository = new TodoPointRepository(db);
                return _todoPointRepository;
            }
        }
        /*
        public void Save()
        {
            db.SaveChanges();
        }*/
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

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
