using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace P01_StudentSystem.Data.Models.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.Name)
            .HasMaxLength(80);

            builder.Property(c => c.Description)
                .IsRequired(false);

            builder.Property(c => c.Price)
                 .HasPrecision(18, 2);
        }
    }
}