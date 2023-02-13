namespace ToDoListApp
{
    public class TodoUpdate
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; } = false;
    }



}
