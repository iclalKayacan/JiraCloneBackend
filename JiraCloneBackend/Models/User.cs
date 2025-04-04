using System.ComponentModel.DataAnnotations;

namespace JiraCloneBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // "Admin", "ProjectOwner", "User"

        public ICollection<UserProject> UserProjects { get; set; }

    }
}
