namespace TaskMasterPRO.Model
{
    public class Task
    {
        public int Id { get; init; }
        public DateTime CreationTime { get; init; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public enum Priority 
    {
        Low, Medium, High
    }
}
