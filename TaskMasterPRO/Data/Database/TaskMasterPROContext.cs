using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data.Domain;

namespace TaskMasterPRO.Data.Database
{
    public class TaskMasterPROContext : DbContext

    {
        public TaskMasterPROContext(DbContextOptions<TaskMasterPROContext> options) : base(options) { }
        public TaskMasterPROContext() { }
        
        public DbSet<Domain.Task> Task { get; set; }
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
