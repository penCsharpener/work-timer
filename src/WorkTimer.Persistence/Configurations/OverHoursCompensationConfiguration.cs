using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations
{
    public class OverHoursCompensationConfiguration : IEntityTypeConfiguration<OverHoursCompensation>
    {
        public void Configure(EntityTypeBuilder<OverHoursCompensation> builder)
        {
            builder.Property(x => x.DateFrom).IsRequired();
            builder.Property(x => x.DateTo).IsRequired();
            builder.Property(x => x.ContractId).IsRequired();

            builder.HasOne(x => x.Contract).WithMany(x => x.OverHoursCompensations).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
