namespace JiraCloneBackend.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Column> Columns { get; set; } = new List<Column>();

        public ICollection<UserProject> UserProjects { get; set; }

    }
}
