namespace JiraCloneBackend.Models
{
    public class TaskItemAssignment
    {
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }

}
