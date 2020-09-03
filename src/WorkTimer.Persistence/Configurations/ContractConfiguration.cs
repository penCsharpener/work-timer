using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations {
    public class ContractConfiguration : IEntityTypeConfiguration<Contract> {
        public void Configure(EntityTypeBuilder<Contract> builder) {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Employer).IsRequired();
            builder.Property(x => x.HoursPerWeek).IsRequired();

            builder.HasMany(x => x.WorkDays).WithOne(x => x.Contract).HasForeignKey(x => x.ContractId);
            builder.HasOne(x => x.User).WithMany(x => x.Contracts).HasForeignKey(x => x.UserId);
        }
    }
}
