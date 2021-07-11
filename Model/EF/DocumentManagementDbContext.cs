using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Model.EF
{
    public partial class DocumentManagementDbContext : DbContext
    {
        public DocumentManagementDbContext()
            : base("name=DocumentManagementDbContext")
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentBook> DocumentBooks { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Credential>()
                .Property(e => e.UserGroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Credential>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.DocumentTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.DocumentBookID)
                .IsUnicode(false);

            modelBuilder.Entity<DocumentBook>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<DocumentType>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.GroupID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.DepartmentID)
                .IsUnicode(false);

            modelBuilder.Entity<UserGroup>()
                .Property(e => e.ID)
                .IsUnicode(false);
        }
    }
}
