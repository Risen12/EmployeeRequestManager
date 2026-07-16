using EmployeeRequestManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRequestManager.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.FullName)
            .HasMaxLength(70);
        
        builder.Property(e => e.Department)
            .HasMaxLength(70);
        
        builder.Property(e => e.Post)
            .HasMaxLength(70);
        
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}