using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;
using WorkTimer.Domain.Models.Enums;

namespace WorkTimer.Persistence.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Priority).IsRequired().HasDefaultValue(TodoPriority.Medium);

        builder.HasOne(x => x.AppUser).WithMany(x => x.Todos).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Contract).WithMany(x => x.Todos).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
    }
}
