using System.Text.Json.Serialization;

namespace JiraCloneBackend.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ColumnId { get; set; }

        [JsonIgnore]
        public Column Column { get; set; } = null!;
    }
}
