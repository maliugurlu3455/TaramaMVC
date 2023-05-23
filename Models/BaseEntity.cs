using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

    }
}
