using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
    }
}
