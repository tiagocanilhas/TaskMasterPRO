using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Model;

namespace TaskMasterPRO.Data
{
    public class TaskMasterPROContext(
        DbContextOptions<TaskMasterPROContext> options
        ) : DbContext(options)
    {
        public DbSet<Model.Task> Task { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=tasks.db");
            }
        }
    }
}
