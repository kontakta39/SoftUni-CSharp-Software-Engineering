using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(100);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(s => s.Birthday)
            .IsRequired(false);
    }
}
