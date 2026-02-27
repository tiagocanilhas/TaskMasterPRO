using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Model;

namespace TaskMasterPRO.Data
{
    public class TaskMasterPROContext : DbContext

    {
        public TaskMasterPROContext(DbContextOptions<TaskMasterPROContext> options) : base(options) { }
        public TaskMasterPROContext() { }
        
        public DbSet<Model.Task> Task { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(DatabaseConfig.GetConnectionString());
            }
        }
    }
}
