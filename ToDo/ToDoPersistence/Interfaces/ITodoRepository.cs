using System;
using System.Collections.Generic;
using System.Text;
using ToDoPersistence.Entities;

namespace ToDoPersistence.Interfaces
{
    public interface ITodoRepository: IRepository<Todo>
    {
    }
}
