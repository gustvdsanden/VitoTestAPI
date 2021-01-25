using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<BoxUser> BoxUsers { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Monitoring> Monitorings { get; set; }
        public DbSet<SensorBox> SensorBoxes { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Location> Locations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserType>().ToTable("UserType");
            modelBuilder.Entity<BoxUser>().ToTable("BoxUser");
            modelBuilder.Entity<Box>().ToTable("Box");
            modelBuilder.Entity<Sensor>().ToTable("Sensor");
            modelBuilder.Entity<Monitoring>().ToTable("Monitoring");
            modelBuilder.Entity<SensorBox>().ToTable("SensorBox");
            modelBuilder.Entity<SensorBox> ().HasKey(sb => new { sb.BoxID, sb.SensorID });
            modelBuilder.Entity<SensorBox>().HasOne(sb => sb.Box).WithMany(b => b.SensorBoxes);
            modelBuilder.Entity<SensorType>().ToTable("SensorType");
            modelBuilder.Entity<Measurement>().ToTable("Measurement");
            modelBuilder.Entity<Measurement>().HasOne(sb => sb.SensorBox).WithMany(m => m.Measurements).HasForeignKey(sb=>new { sb.SensorID,sb.BoxID});
            modelBuilder.Entity<Location>().ToTable("Location");




        }
    }
}
