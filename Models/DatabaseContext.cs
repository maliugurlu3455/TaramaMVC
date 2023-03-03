using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
       
      

    }
}
