using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaramaMVC.Models
{
    public class PersonelMap : IEntityTypeConfiguration<Personel>
    {
        public void Configure(EntityTypeBuilder<Personel> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(a => a.AnaBilimDali)
                .WithMany(p => p.Personels)
                .HasForeignKey(x => x.AId)
                .OnDelete(DeleteBehavior.Cascade);
            // data vermek için
            //builder.HasData(new )

        }
    }
    public class AnaBilimDaliMap : IEntityTypeConfiguration<AnaBilimDali>
    {
        public void Configure(EntityTypeBuilder<AnaBilimDali> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            // data vermek için
            //builder.HasData(new  )

        }
    }
}
