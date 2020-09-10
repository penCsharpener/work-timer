using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations {
    public class WorkDayConfiguration : IEntityTypeConfiguration<WorkDay> {
        public void Configure(EntityTypeBuilder<WorkDay> builder) {
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.ContractId).IsRequired();

            builder.HasOne(x => x.Contract).WithMany(x => x.WorkDays).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
