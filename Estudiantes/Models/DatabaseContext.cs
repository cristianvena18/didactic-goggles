using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Models
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentSubject>().HasKey(x => new { x.StudentId, x.SubjectId });

            modelBuilder.Entity<IdentityUser>().ToTable("Users", "Security");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Security");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims", "Security");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "Security");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "Security");


        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<StudentSubject> StudentSubjects{ get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }
}
