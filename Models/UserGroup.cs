using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class UserGroup
    {
        [Key]
        public int UserGroupId { get; set; }

        public int UserId { get; set; }
        public Users Users { get; set; } = null!;

        public int GroupId { get; set; }
        public Groups Groups { get; set; } = null!;
    }
}
