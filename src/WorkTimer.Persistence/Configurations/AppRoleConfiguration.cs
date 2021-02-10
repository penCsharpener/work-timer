using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Configurations {
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole> {
        public void Configure(EntityTypeBuilder<AppRole> builder) {
            builder.HasData(new AppRole { Name = "User", NormalizedName = "USER", Id = 1, ConcurrencyStamp = Guid.NewGuid().ToString() });
            builder.HasData(new AppRole { Name = "Admin", NormalizedName = "ADMIN", Id = 2, ConcurrencyStamp = Guid.NewGuid().ToString() });
        }
    }
}