using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations {
    public class WorkingPeriodConfiguration : IEntityTypeConfiguration<WorkingPeriod> {
        public void Configure(EntityTypeBuilder<WorkingPeriod> builder) {
            builder.Property(x => x.StartTime).IsRequired();

            builder.HasOne(x => x.WorkDay).WithMany(x => x.WorkingPeriods).HasForeignKey(x => x.WorkDayId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
