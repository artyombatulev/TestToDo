using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoPersistence.Entities;

namespace ToDoPersistence.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ITodoRepository TodoRep { get; }
        ITodoPointRepository TodoPointRep { get; }
        Task Save();
    }
}
