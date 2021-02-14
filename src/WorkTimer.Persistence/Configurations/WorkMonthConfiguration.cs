using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations
{
    public class WorkMonthConfiguration : IEntityTypeConfiguration<WorkMonth>
    {
        public void Configure(EntityTypeBuilder<WorkMonth> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.Month, x.Year }).IsUnique();
        }
    }
}
