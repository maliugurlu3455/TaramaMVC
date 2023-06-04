using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class Personel: BaseEntity
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string SurName { get; set; }

        public int AnaBilimDallariId { get; set; } = 0;
        public virtual AnaBilimDali? AnaBilimDallari { get; set; } 

        [StringLength(50)]
        //[Required]
        public string User { get; set; } = "";
        public int Alintilanma { get; set; } = 0;
        [StringLength(150)]
        public string ScholarName { get; set; } = "";

        public int? h_endex { get; set; }
        public int? i10_endex { get; set; }

    }
}
