using VeterinaryPassport.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VeterinaryPassport.Models
{
    public class Context : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin";
            string adminPassword = "admin";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Login = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
