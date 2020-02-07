using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WorkTimer.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int> {

        public AppDbContext([NotNull] DbContextOptions options) : base(options) {

        }

        protected AppDbContext() {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
        }
    }
}
