using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaramaMVC.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaramaMVC.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {
           
        }
        public DbSet<PersonelYayinBilgileri> PersonelYayinBilgileris { get; set; } = default!;
        public DbSet<Personel> Personels { get; set; } = default!;
        public DbSet<AnaBilimDali> AnaBilimDals { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonelYayinBilgileri>().ToTable("PersonelYayinBilgileris");

            modelBuilder.Entity<AnaBilimDali>()
     .HasMany(c => c.Personeller)
     .WithOne(e => e.AnaBilimDallari);

            modelBuilder.Entity<Personel>()
        .HasOne(e => e.AnaBilimDallari)
        .WithMany(c => c.Personeller);

           

        }

       


    }
}
