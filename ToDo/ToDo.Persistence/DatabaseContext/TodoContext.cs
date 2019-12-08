using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDo.DataAccessModels;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.Persistence.DatabaseContext
{
    public class TodoContext : DbContext, ITodoContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoPoint> TodoPoints { get; set; }

        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<T> DbSet<T>() where T : BaseEntity
        {
            return Set<T>();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
