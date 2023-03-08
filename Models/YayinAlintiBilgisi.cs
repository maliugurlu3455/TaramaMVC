using System.ComponentModel.DataAnnotations.Schema;

namespace TaramaMVC.Models
{
    public class YayinAlintiBilgisi
    {
        public int Id { get; set; }

        [Column("YayinId")]
        public int personelYayinBilgileriId { get; set; }
        public PersonelYayinBilgileri personelYayinBilgileri { get; set; }
        public string Tip { get; set; }
        public string Ad { get; set; }
    }
}
