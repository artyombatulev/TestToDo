using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.Persistence.DatabaseContext
{
    public interface ITodoContext
    {
        DbSet<T> DbSet<T>() where T : BaseEntity;

        Task SaveAsync();

        void Dispose();
    }
}
