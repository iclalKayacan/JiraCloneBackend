using System.Text.Json.Serialization;

namespace JiraCloneBackend.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; } 
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

}
