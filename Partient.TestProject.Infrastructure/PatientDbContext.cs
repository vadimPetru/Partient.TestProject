using Microsoft.EntityFrameworkCore;
using Partient.TestProject.Domain.Models;
using Partient.TestProject.Infrastructure.Configuration;

namespace Partient.TestProject.Infrastructure
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }
        public PatientDbContext()
        {
        }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("btree_gist");
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
