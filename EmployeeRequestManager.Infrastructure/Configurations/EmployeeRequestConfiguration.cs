using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRequestManager.Infrastructure.Configurations;

public class EmployeeRequestConfiguration : IEntityTypeConfiguration<EmployeeRequest>
{
    public void Configure(EntityTypeBuilder<EmployeeRequest> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.Executor)
            .WithMany()
            .HasForeignKey(a => a.ExecutorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(e => e.Status)
            .HasConversion(new StatusConverter())
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(e => e.Description)
            .HasMaxLength(500);
    }
}