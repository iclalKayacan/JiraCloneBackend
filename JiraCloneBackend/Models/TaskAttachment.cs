using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JiraCloneBackend.Models
{
    public class TaskAttachment
    {
        [Key]
        public int Id { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
