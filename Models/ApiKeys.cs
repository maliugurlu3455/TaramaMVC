using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class ApiKeys
    {
        [Key]
        public int Id { get; set; } 
        public string? Key { get; set; }
        public int? Sayi { get; set; } = 0;
        public bool? IsTamam { get; set; } = false;
    }
}
