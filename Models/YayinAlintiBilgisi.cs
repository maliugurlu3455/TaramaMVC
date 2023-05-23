using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaramaMVC.Models
{
    public class YayinAlintiBilgisi: BaseEntity
    {
        [Column("YayinId")]
        public int personelYayinBilgileriId { get; set; }
        public PersonelYayinBilgileri personelYayinBilgileri { get; set; }
        //public string? Tip { get; set; }
        public string Ad { get; set; }

        private string tip = "APA";
        public string Tip { get =>tip; set => tip = string.IsNullOrEmpty(value) ? "APA": value; }

        [StringLength(500)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? Link { get; set; }
        [StringLength(4000)]
        public string? Snippet { get; set; }
        [StringLength(500)]
        public string? PublicationInfo { get; set; }
        [StringLength(500)]
        public string? Resource { get; set; }
        public int status { get; set; } = 0;
        public string? SID { get; set; }
    }
}
