using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;
using WorkTimer.Domain.Models.Enums;

namespace WorkTimer.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasDefaultValue(TodoPriority.Medium);

        builder.HasOne(x => x.AppUser).WithMany(x => x.Notes).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Contract).WithMany(x => x.Notes).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
    }
}