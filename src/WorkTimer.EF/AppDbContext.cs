using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WorkTimer.EF.Models;

namespace WorkTimer.EF {
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int> {

        public AppDbContext([NotNull] DbContextOptions options) : base(options) {

        }

        protected AppDbContext() {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
        }
    }
}
