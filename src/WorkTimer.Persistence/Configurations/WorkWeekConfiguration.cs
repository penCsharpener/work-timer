using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkTimer.Domain.Models.Configurations
{
    public class WorkWeekConfiguration : IEntityTypeConfiguration<WorkWeek>
    {
        public void Configure(EntityTypeBuilder<WorkWeek> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.WeekStart }).IsUnique();
        }
    }
}
