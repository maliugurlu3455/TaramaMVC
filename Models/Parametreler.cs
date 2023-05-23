using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaramaMVC.Models
{
    public class Parametreler
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int GrupId { get; set; }
        public Grup? Grup { get; set; }
        public bool IsGizli { get; set; } = false;

    }
    public class Grup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
      
    }
}
