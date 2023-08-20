using Microsoft.EntityFrameworkCore;

namespace UserManagement.Models
{
    public class UserManagementContext : DbContext
    {
        public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Groups>().HasData(
            //new Groups { GroupId = 1, GroupName = "Group1" },
            //new Groups { GroupId = 2, GroupName = "Group2" },
            //new Groups { GroupId = 3, GroupName = "Group3" }
        //);

            // Configure many-to-many relationship between User and Group through UserGroup
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .Property(ug => ug.UserGroupId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Users)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Groups)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasKey(gp => new { gp.GroupId, gp.PermissionId });

            modelBuilder.Entity<GroupPermission>()
                .Property(gp => gp.GroupPermissionId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Groups)
                .WithMany()
                .HasForeignKey(gp => gp.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany()
                .HasForeignKey(gp => gp.PermissionId);
                
        }
    }
}
