using Microsoft.Build.Evaluation;

namespace TaramaMVC.Models
{
    public class PersonelYayinBilgileri: BaseEntity
    { 
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public string Baslik { get; set; }
        public string BaslikCites { get; set; }
        public int Alinti { get; set; }
        public int Yil { get; set; }
        
        public DateTime UpdateDate { get; set; }
    }
}
