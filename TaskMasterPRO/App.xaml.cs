using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using TaskMasterPRO.Data;
using TaskMasterPRO.Model;
using TaskMasterPRO.Repository;
using TaskMasterPRO.Services;
using TaskMasterPRO.ViewModel;

namespace TaskMasterPRO
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup DbContext
            DbContextOptionsBuilder<TaskMasterPROContext> optionsBuilder = new();
            optionsBuilder.UseSqlite(DatabaseConfig.GetConnectionString());
            TaskMasterPROContext context = new(optionsBuilder.Options);

            // Domains
            CategoryDomain categoryDomain = new();
            TaskDomain taskDomain = new();

            // Repositories
            CategoryRepository categoryRepository = new(context);
            TaskRepository taskRepository = new(context);

            // Services
            DialogServices dialogServices = new();
            CategoryServices categoryServices = new(categoryDomain, categoryRepository);
            TaskServices taskServices = new(taskDomain, taskRepository);

            // ViewModel
            MainViewModel vm = new(dialogServices, categoryServices, taskServices);

            // Main Window
            MainWindow mainWindow = new();
            mainWindow.DataContext = vm;

            _ = vm.LoadContent();
            mainWindow.Show();
        }
    }

}
