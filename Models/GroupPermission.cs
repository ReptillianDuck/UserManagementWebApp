using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class GroupPermission
    {
        [Key] 
        public int GroupPermissionId { get; set; }

        public int GroupId { get; set; }
        public Groups Groups { get; set; } = null!;

        public int PermissionId { get; set; }   
        public Permission Permission { get; set; } = null!;
    }
}
