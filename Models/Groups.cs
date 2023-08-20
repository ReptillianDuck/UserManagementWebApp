using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class Groups
    {
        [Key]
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        
        public List<UserGroup> UserGroups { get; set; }

        public Groups() 
        { 
            UserGroups = new List<UserGroup>();
        }
    }
}
