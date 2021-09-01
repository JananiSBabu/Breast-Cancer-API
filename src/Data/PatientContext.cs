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

        // Add initial data(seeding) to the database
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Order>()
            //    .HasData(new Order()
            //    {
            //        Id = 1,
            //        OrderDate = System.DateTime.UtcNow,
            //        OrderNumber = "12345"
            //    });
        }

        // Add initial data(seeding) to the database
        public DbSet<BreastCancerAPI.Models.PatientModel> PatientModel { get; set; }
    }
}
