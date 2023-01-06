using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaramaMVC.Models;
namespace TaramaMVC.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {
           
        }
        public DbSet<Personel> Personels { get; set; } = default!;
        public DbSet<AnaBilimDali> AnaBilimDals { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personel>().ToTable("Personels");
            modelBuilder.Entity<AnaBilimDali>().ToTable("AnaBilimDalis");

        }
      
   
        

    }
}
