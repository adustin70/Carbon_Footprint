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

            builder.Entity<CarbonFootprint>()
                .HasData(
                new CarbonFootprint { Id = 1, MilesDrivenData = 0, FuelTypeData = "Gasoline", PlasticBagsUsedData = 0, PlasticBottlesUsedData = 0, PowerUsedData = 0 },
                new CarbonFootprint { Id = 2, MilesDrivenData = 0, FuelTypeData = "Diesel", PlasticBagsUsedData = 0, PlasticBottlesUsedData = 0, PowerUsedData = 0 },
                new CarbonFootprint { Id = 3, MilesDrivenData = 0, FuelTypeData = "Electric", PlasticBagsUsedData = 0, PlasticBottlesUsedData = 0, PowerUsedData = 0}
                );
        }

        public DbSet<Environmentalist> Environmentalists { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<CarbonFootprint> CarbonFootprints { get; set; }
    }
}
