using EntityFrameworkClassLibrary.Models;

namespace ToDoListApp
{
    public class Todo : Timestamps
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
