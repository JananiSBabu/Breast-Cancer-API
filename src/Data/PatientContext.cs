using BreastCancerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreastCancerAPI.Models;

namespace BreastCancerAPI.Data
{
    public class PatientContext : DbContext
    {
        private readonly IConfiguration _config;

        public PatientContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PrognosticInfo> PrognosticInfos { get; set; }
        public DbSet<CellFeatures> CellFeatures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Specify Connection string to DbContext
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PatientContextDb"));

        }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Patient -> PrognosticInfo -> Patient 
            // Use "DeleteBehavior.Cascade": Deleting Patient will delete all the related PrognosticInfo 
            //https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#configuring-cascading-behaviors 
            builder.Entity<Patient>()
            .HasMany(i => i.PrognosticInfos)
            .WithOne(c => c.Patient)
            .OnDelete(DeleteBehavior.Cascade);

            // Add simple initial data(seeding) to the database, if needed
        }

        // Add initial data(seeding) to the database
        public DbSet<BreastCancerAPI.Models.PatientModel> PatientModel { get; set; }

        // Add initial data(seeding) to the database
        public DbSet<BreastCancerAPI.Models.PrognosticInfoModel> PrognosticInfoModel { get; set; }
    }
}
