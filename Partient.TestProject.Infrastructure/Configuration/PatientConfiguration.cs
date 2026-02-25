using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Infrastructure.Configuration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> entity)
        {
            entity.HasKey(p => p.Name.Id);

            entity.OwnsOne(e => e.Name, name =>
            {
                name.Property(n => n.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");

                name.Property(n => n.Family)
                .IsRequired()
                .HasMaxLength(100);

                name.Property(n => n.Use)
                .HasMaxLength(50);

                name.Property(n => n.Given)
                .HasConversion(v => string.Join(',', v),
                            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );
            });

            entity.Property(e => e.BirthDate)
                   .IsRequired();

            entity.Property(e => e.Gender)
                .HasConversion<string>();

            entity.Property(e => e.Active);

            entity.HasIndex(e => e.BirthDate)
                .HasDatabaseName("IX_Patients_BirthDate");
        }
    }
}
