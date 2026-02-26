using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Partient.TestProject.Domain.Models;
using Partient.TestProject.Infrastructure.Converters;

namespace Partient.TestProject.Infrastructure.Configuration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> entity)
        {
            entity.HasKey(p => p.Id);

            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Id)
                    .HasColumnName("Id");

                name.Property(n => n.Family)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Family");

                name.Property(n => n.Use)
                    .HasMaxLength(50)
                    .HasColumnName("Use");

                name.Property(n => n.Given)
                    .HasColumnType("jsonb")
                    .HasColumnName("Given")
                    .HasConversion(new JsonArrayConverter());
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
