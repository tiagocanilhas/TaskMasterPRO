namespace TaskMasterPRO.Model
{
    public class Task
    {
        public int Id { get; init; }
        public DateTime CreationTime { get; init; }

        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime Deadline { get; init; }
        public Boolean IsCompleted { get; init; }
        public Priority Priority { get; init; }
        public int CategoryId { get; init; }
        public Category Category { get; init; }
    }

    public enum Priority 
    {
        Low, Medium, High
    }
}
