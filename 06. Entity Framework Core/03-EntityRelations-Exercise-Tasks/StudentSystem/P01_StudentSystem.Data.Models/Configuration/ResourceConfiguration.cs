using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace P01_StudentSystem.Data.Models.Configuration;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.Property(r => r.Name)
                .HasMaxLength(50);

        builder.Property(r => r.Url)
                .IsUnicode(false);
    }
}