using Microsoft.EntityFrameworkCore;
using System.Windows;
using TaskMasterPRO.Data;
using TaskMasterPRO.Model;
using TaskMasterPRO.Repository;
using TaskMasterPRO.Services;
using TaskMasterPRO.ViewModel;

namespace TaskMasterPRO
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
            CategoryServices categoryServices = new(categoryDomain, categoryRepository);
            TaskServices taskServices = new(taskDomain, taskRepository);

            // ViewModel
            MainViewModel vm = new(categoryServices, taskServices);

            DataContext = vm;
            vm.LoadTasksAsync();
        }
    };

}