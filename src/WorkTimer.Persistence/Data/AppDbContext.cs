using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkTimer.Domain.Models;

namespace WorkTimer.Persistence.Data {
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }



        public DbSet<WorkingPeriod> WorkingPeriods { get; set; }
        public DbSet<WorkDay> WorkingDays { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
