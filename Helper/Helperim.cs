using HtmlAgilityPack;
using NetTopologySuite.Index.HPRtree;
using TaramaMVC.Models;

namespace TaramaMVC.Helper
{
    public static class Helperim
    {
        public static string  GetUserId(string Ad, string Soyad)
        {
            HtmlDocument doc = null;
            string UserID = null;
            try
            {
                HtmlWeb web = new HtmlWeb();
                var url = new Uri("https://scholar.google.com/citations?hl=tr&view_op=search_authors&mauthors="+ Ad + "+"+ Soyad + "&btnG=");
                doc = web.Load(url);
                var nod = doc.DocumentNode.SelectSingleNode("//h3[@class = 'gs_ai_name']");
                if (nod != null)
                {
                var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                int aranan = nodesWithARef.IndexOf("user=", 0);
                UserID = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));   

                }
                
            }
            catch
            {
                UserID = null;
            }
            return UserID;
        }
        public static string GetUserId(string AdSoyad)
        {
            HtmlDocument doc = null;
            string UserID = string.Empty;
            try
            {
                HtmlWeb web = new HtmlWeb();
                var url = new Uri("https://scholar.google.com/citations?hl=tr&view_op=search_authors&mauthors=" + AdSoyad + "&btnG=");
                doc = web.Load(url);
                var nod = doc.DocumentNode.SelectSingleNode("//h3[@class = 'gs_ai_name']");
                if (nod != null)
                {
                    var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    int aranan = nodesWithARef.IndexOf("user=", 0);
                    UserID = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));
                    
                   // string value = nod.SelectSingleNode("//span[@class='gs_hlt']").InnerText;

                }

            }
            catch
            {
                UserID = string.Empty; ;
            }
            return UserID;
        }
        public static string[] GetUserId(string AdSoyad,bool hepsi)
        {
            HtmlDocument doc = null;
            string[] UserID = new string[] { string.Empty, string.Empty, string.Empty};
            try
            {
                HtmlWeb web = new HtmlWeb();
                var url = new Uri("https://scholar.google.com/citations?hl=tr&view_op=search_authors&mauthors=" + AdSoyad + "&btnG=");
                doc = web.Load(url);
                var nod = doc.DocumentNode.SelectSingleNode("//div[@class = 'gs_ai_t']");
                if (nod != null)
                {
                    var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    int aranan = nodesWithARef.IndexOf("user=", 0);
                    UserID[0] = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));
                    string value = nod.SelectSingleNode("//div[@class='gs_ai_cby']").InnerText.Replace("Alıntılanma sayısı: ","").Trim();
                    UserID[1] = value;
                    // string value = nod.SelectSingleNode("//div[@class='gs_ai_cby']").InnerText;

                }

            }
            catch
            {
                UserID = new string[] { string.Empty, string.Empty, string.Empty };
            }
            return UserID;
        }
        public static List<PersonelYayinBilgileri> KullaniciBilgileri(string users)
        {
            HtmlDocument doc = null;
            List<PersonelYayinBilgileri> Liste = new List<PersonelYayinBilgileri>();
            PersonelYayinBilgileri pers = null;
            string baslik = "";
            string baslikcites = "";
            string alintilanma = "";
            string yil = "";
            string alintitoplam = "";
            try
            {
                HtmlWeb web = new HtmlWeb();
                var url = new Uri("https://scholar.google.com/citations?hl=tr&pagesize=1000&user=" + users.Trim() + "");
                doc = web.Load(url);
                //alıntı sayısı

                //var ScholarName = doc.DocumentNode.SelectNodes("//div[@class='gs_scl']");
                //var rows = doc.DocumentNode.SelectNodes("//tr");
                //var table1 = doc.DocumentNode.SelectNodes("//table").First();
                //var trs = doc.DocumentNode.Descendents("//table[@class='tablehead']/tr/");
                var nods = doc.DocumentNode.SelectNodes("//tr[@class = 'gsc_a_tr']");
                var alintitoplamnod = doc.DocumentNode.SelectSingleNode("//div[@class ='gsc_rsb_s gsc_prf_pnl']");
                if (alintitoplamnod!=null)
                {
                     alintitoplam = string.IsNullOrEmpty(alintitoplamnod.SelectSingleNode("//td[@class = 'gsc_rsb_std']").InnerText) ? "0" : alintitoplamnod.SelectSingleNode("//td[@class = 'gsc_rsb_std']").InnerText;

                }
                if (nods != null)
                {
                    for (int i = 0; i < nods.Count-1; i++)
                    {
                        pers = new PersonelYayinBilgileri();
                        pers.Personel = new Personel();
                        pers.Personel.Alintilanma = Convert.ToInt32(alintitoplam);
                        baslik = nods[i].SelectNodes("//a[@class='gsc_a_at']")[i].InnerText;//nods[i].SelectSingleNode("//a[@class='gsc_a_at']").InnerText; gsc_a_ac gs_ibl

                        if(nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']").Count > 0)
                        {

                            baslikcites= nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].InnerText;
                        }
                        else
                        {
                            baslikcites = "";
                        }

                        alintilanma = string.IsNullOrEmpty(nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].InnerText)?"0": nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].InnerText;//string.IsNullOrEmpty(nods[i].SelectSingleNode("//a[@class='gsc_a_ac gs_ibl']").InnerText) ? "0" : nods[i].SelectSingleNode("//a[@class='gsc_a_ac gs_ibl']").InnerText;
                        yil = string.IsNullOrEmpty(nods[i].SelectNodes("//span[@class='gsc_a_h gsc_a_hc gs_ibl']")[i].InnerText) ? "0" : nods[i].SelectNodes("//span[@class='gsc_a_h gsc_a_hc gs_ibl']")[i].InnerText; //string.IsNullOrEmpty(nods[i].SelectSingleNode("//span[@class='gsc_a_h gsc_a_hc gs_ibl']").InnerText) ? "0" : nods[i].SelectSingleNode("//span[@class='gsc_a_h gsc_a_hc gs_ibl']").InnerText;
                        pers.Baslik = baslik;
                        pers.Alinti = Convert.ToInt32(alintilanma);
                        pers.Yil = Convert.ToInt32(yil);
                        pers.BaslikCites = baslikcites;
                        Liste.Add(pers);
                    } 
                        
                 
                    //var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    //int aranan = nodesWithARef.IndexOf("user=", 0);
                    //UserID[0] = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));
                  //  string value = nod.SelectSingleNode("//div[@class='gs_ai_cby']").InnerText.Replace("Alıntılanma sayısı: ", "").Trim();
                  //  UserID[1] = value;
                    // string value = nod.SelectSingleNode("//div[@class='gsc_prf_in']").InnerText;

                }

            }
            catch
            {
                Liste = null;
            }
            return Liste;
        }
    }
}
