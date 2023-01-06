using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string SurName { get; set; }

        public int AId { get; set; }
        public AnaBilimDali AnaBilimDali { get; set; }
  

    }
}
