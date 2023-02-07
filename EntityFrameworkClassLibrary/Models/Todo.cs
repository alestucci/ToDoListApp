namespace ToDoListApp
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; } = false;
    }

}
