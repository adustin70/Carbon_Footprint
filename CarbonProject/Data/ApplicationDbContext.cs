using CarbonProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Name = "Environmentalist",
                    NormalizedName = "ENVIRONMENTALIST"
                }
            );
        }

        public DbSet<Environmentalist> Environmentalists { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<CarbonFootprint> CarbonFootprints { get; set; }
    }
}
