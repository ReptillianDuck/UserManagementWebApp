using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<UserGroup> UserGroups { get; set; }

        public Users()
        {
            UserGroups = new List<UserGroup>();
        }
    }
}
