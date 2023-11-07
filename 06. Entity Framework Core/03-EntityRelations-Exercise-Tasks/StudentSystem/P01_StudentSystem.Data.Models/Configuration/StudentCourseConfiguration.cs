using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configuration
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(pk => new { pk.StudentId, pk.CourseId });

            builder.HasOne(s => s.Student)
                .WithMany(sc => sc.StudentsCourses)
                .HasForeignKey(fk => fk.StudentId);

            builder.HasOne(c => c.Course)
                .WithMany(sc => sc.StudentsCourses)
                .HasForeignKey(fk => fk.CourseId);
        }
    }
}