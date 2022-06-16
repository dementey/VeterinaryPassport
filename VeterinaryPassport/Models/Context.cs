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
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
