using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace TaramaMVC.Models
{
    public class DatabaseContext: IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {
           
        }
        public DbSet<YayinAlintiBilgisi> YayinAlintiBilgisis { get; set; } = default!;
        public DbSet<PersonelYayinBilgileri> PersonelYayinBilgileris { get; set; } = default!;
        public DbSet<Personel> Personels { get; set; } = default!;
        public DbSet<AnaBilimDali> AnaBilimDals { get; set; } = default!;
        public DbSet<ApiKeys> ApiKey { get; set; } = default!;
        public DbSet<Grup> Grups { get; set; } = default!;
        public DbSet<Parametreler> Parametrelers { get; set; } = default!;
        //public DbSet<OrganicResult> OrganicResults { get; set; } = default!;

        //public DbSet<Organic_Result> Organic_Results { get; set; } = default!;
        //public DbSet<Publication_Info> Publication_Infos { get; set; } = default!;
        //public DbSet<Author> Authors { get; set; } = default!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<YayinAlintiBilgisi>()
        //        .Property(b => b.Tip)
        //        .HasDefaultValueSql("APA");OrganicResult
        //}

    }
}
