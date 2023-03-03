using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string SurName { get; set; }

        public int AnaBilimDallariId { get; set; }
        public  AnaBilimDali AnaBilimDallari { get; set; }

        [StringLength(50)]
        [Required]
        public string User { get; set; }
        public int Alintilanma { get; set; }
        [StringLength(150)]
        public string ScholarName { get; set; }

        
    }
}
