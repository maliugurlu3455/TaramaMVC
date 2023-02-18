using System.ComponentModel.DataAnnotations.Schema;

namespace TaramaMVC.Models
{
    public class YayinAlintiBilgisi
    {
        public int Id { get; set; }

        
        public int YayinId { get; set; }
        public string Tip { get; set; }
        public string Ad { get; set; }
    }
}
