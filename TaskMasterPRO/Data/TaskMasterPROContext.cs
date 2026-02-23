using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Model;

namespace TaskMasterPRO.Data
{
    public class TaskMasterPROContext : DbContext
    {
        public DbSet<Model.Task> Tasks { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tasks.db");
        }
    }
}
