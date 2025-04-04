namespace JiraCloneBackend.Dto
{
    public class TaskItemCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ColumnId { get; set; }

        public List<int> UserIds { get; set; }

    }
}
