using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations;

public class WorkWeekConfiguration : IEntityTypeConfiguration<WorkWeek>
{
    public void Configure(EntityTypeBuilder<WorkWeek> builder)
    {
        builder.HasIndex(x => new { x.ContractId, x.WeekStart }).IsUnique();
    }
}
