using Microsoft.EntityFrameworkCore;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Infrastructure
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
    }
}
