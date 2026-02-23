using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskMasterPRO.Data;
using TaskMasterPRO.Model;

namespace TaskMasterPRO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetData();

            PrintTaskTable();
        }

        private static void SetData()
        {
            TaskMasterPROContext context = new();

            context.Database.EnsureDeleted(); // Deletes DB
            context.Database.Migrate(); // Builds DB

            Category cat = new()
            {
                Name = "Name",
                Description = "Description",
                Color = "#FFF"
            };

            Model.Task task = new()
            {
                Title = "Title",
                Description = "Description",
                Deadline = DateTime.Now + TimeSpan.FromSeconds(60 * 10),
                IsCompleted = false,
                Priority = Priority.Low,
                Category = cat,
            };

            context.Add(task);

            context.SaveChanges();
        }

        private static void PrintTaskTable()
        {
            TaskMasterPROContext context = new();

            var tasks = from task in context.Tasks select task;

            foreach (var task in tasks) {
                Console.WriteLine($"Id: {task.Id}");
                Console.WriteLine($"CreationTime: {task.CreationTime}");
                Console.WriteLine($"Title: {task.Title}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine($"Deadline: {task.Deadline}");
                Console.WriteLine($"IsCompleted: {task.IsCompleted}");
                Console.WriteLine($"Priority: {task.Priority}");
                Console.WriteLine($"CategoryId: {task.CategoryId}");
                Console.WriteLine($"Category: {task.Category}");
                Console.WriteLine(new string('-', 20));
            }
        }
    };

}