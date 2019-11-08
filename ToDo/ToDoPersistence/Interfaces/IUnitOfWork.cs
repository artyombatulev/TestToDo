using System;
using System.Collections.Generic;
using System.Text;
using ToDoPersistence.Entities;

namespace ToDoPersistence.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Todo> Todos { get; }
        IRepository<TodoPoint> TodoPoints { get; }
        void Save();
    }
}
