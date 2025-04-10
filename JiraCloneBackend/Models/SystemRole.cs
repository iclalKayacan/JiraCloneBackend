namespace JiraCloneBackend.Models
{
    public enum SystemRole
    {
        Admin,         // Sistemin tümüne erişebilir
        ProjectOwner,  // Sadece kendi projelerini yönetebilir
        User           // Sadece kendisine atanan görevleri düzenleyebilir
    }
}
