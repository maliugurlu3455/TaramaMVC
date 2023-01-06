using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class AnaBilimDali
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        public ICollection<Personel> Personels { get; set; }  
    }
}
