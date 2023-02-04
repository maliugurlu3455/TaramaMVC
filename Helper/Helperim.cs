using HtmlAgilityPack;
using NetTopologySuite.Index.HPRtree;
using TaramaMVC.Models;

namespace TaramaMVC.Helper
{
    public static class Helperim
    {
       
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
            System.Threading.Thread.Sleep(Random.Shared.Next(100, 1100));
            HtmlDocument doc = null;
            List<PersonelYayinBilgileri> Liste = new List<PersonelYayinBilgileri>();
            PersonelYayinBilgileri pers = null;
            string baslik = "";
            //string baslikUrl = "";
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
           

                try
                {
                    if (alintitoplamnod != null) { 
                        alintitoplam = alintitoplamnod.SelectSingleNode("//td[@class = 'gsc_rsb_std']").InnerText;
                    }
                    else
                    {
                        alintitoplam = "0";
                    }
                    
                }
                catch (Exception)
                {
                    alintitoplam = "0";

                }
                if (nods != null)
                {
                    for (int i = 0; i < nods.Count; i++)
                    {
                        if (i== nods.Count-1)
                        {
                            baslikcites = "";
                        }
                        baslikcites = "";
                        baslik = "";
                        alintilanma = "0";
                        yil = "0";
                        pers = new PersonelYayinBilgileri();
                        pers.Personel = new Personel();
                        pers.Personel.Alintilanma = Convert.ToInt32(alintitoplam);
                        
                        try
                        {
                            baslik = nods[i].SelectNodes("//a[@class='gsc_a_at']")[i].InnerText;
                        }
                        catch (Exception)
                        {

                            baslik ="";
                        }
                        try
                        {
                            baslikcites = nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].GetAttributeValue("href", "");
                            if (baslikcites!="" && baslikcites!=" ")
                            {
                                baslikcites = baslikcites.Replace("amp;", "");
                            }
                        }
                        catch (Exception)
                        {

                            baslikcites = "";
                        }
                       
                     
                        try
                        {
                            alintilanma = nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].InnerText;
                            if (alintilanma=="")
                            {
                                alintilanma = "0";
                            }

                        }
                        catch (Exception)
                        {
                            alintilanma = "0";


                        }
                        try
                        {
                            yil = nods[i].SelectNodes("//span[@class='gsc_a_h gsc_a_hc gs_ibl']")[i].InnerText;
                            if (yil == "")
                            {
                                yil = "0";
                            }
                        }
                        catch (Exception)
                        {
                            yil = "0";


                        }
                        pers.Baslik = baslik;
                        pers.Alinti = Convert.ToInt32(alintilanma);
                        pers.Yil = Convert.ToInt32(yil);
                        pers.BaslikCites = baslikcites;
                        Liste.Add(pers);
                    } 

                }

            }
            catch(Exception ex)
            {
                string hata = baslikcites+" - "+ ex.Message;
                Liste = null;
            }
            return Liste;
        }

        public static string AlintiGetir(string url)
        {
            //string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = null;
            HtmlDocument doc2 = null;
            string alintiadi = "";
            doc = web.Load(url);
            var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");
            if (nods != null && nods.Count > 0)
            {
                for (int i = 0; i < nods.Count; i++)
            {
                    var baslikUrl = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");
                   
                    if (baslikUrl.Length>4)
                    {
                        string url2 = "https://scholar.google.com/scholar?q=info:"+ baslikUrl + ":scholar.google.com/&output=cite&scirp=0&hl=tr";
                        web = new HtmlWeb();

                        doc2=web.Load(url2,"GET");
                        var list1 = doc2.DocumentNode.SelectNodes("//div[@class = 'gs_citr']");
                        
                        if (list1!=null && list1.Count>0)
                        {
                            //for (int j = 0; i < list1.Count; j++)
                            //{
                                 alintiadi = list1[1].InnerHtml;
                            //}
                            
                        }
                    }
                    
                }
            }
            return alintiadi;   
        }
    }
}
#region eskiler
//if (nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i]!=null && nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']").Count > 0)
//{
//    try
//    {
//        baslikcites = nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].InnerText;

//        //var nodesWithARef = nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].GetAttributeValue("href", "");
//        //if (nodesWithARef!="" && nodesWithARef.Length>80)
//        //{
//        //    int aranan = nodesWithARef.IndexOf("cites=", 0);
//        //    baslikcites = nodesWithARef.Substring(aranan + 6, nodesWithARef.Length - (aranan + 6));

//        //}
//        //else
//        //{
//        //    baslikcites = "";
//        //}

//    }
//    catch (Exception e)
//    {
//        baslikcites = e.Message;
//    }

//}
//else
//{
//    baslikcites = "";
//}
//public static string  GetUserId(string Ad, string Soyad)
//{
//    HtmlDocument doc = null;
//    string UserID = null;
//    try
//    {
//        HtmlWeb web = new HtmlWeb();
//        var url = new Uri("https://scholar.google.com/citations?hl=tr&view_op=search_authors&mauthors="+ Ad + "+"+ Soyad + "&btnG=");
//        doc = web.Load(url);
//        var nod = doc.DocumentNode.SelectSingleNode("//h3[@class = 'gs_ai_name']");
//        if (nod != null)
//        {
//        var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
//        int aranan = nodesWithARef.IndexOf("user=", 0);
//        UserID = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));   

//        }

//    }
//    catch
//    {
//        UserID = null;
//    }
//    return UserID;
//}
//public static string GetUserId(string AdSoyad)
//{
//    HtmlDocument doc = null;
//    string UserID = string.Empty;
//    try
//    {
//        HtmlWeb web = new HtmlWeb();
//        var url = new Uri("https://scholar.google.com/citations?hl=tr&view_op=search_authors&mauthors=" + AdSoyad + "&btnG=");
//        doc = web.Load(url);
//        var nod = doc.DocumentNode.SelectSingleNode("//h3[@class = 'gs_ai_name']");
//        if (nod != null)
//        {
//            var nodesWithARef = nod.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
//            int aranan = nodesWithARef.IndexOf("user=", 0);
//            UserID = nodesWithARef.Substring(aranan + 5, nodesWithARef.Length - (aranan + 5));

//           // string value = nod.SelectSingleNode("//span[@class='gs_hlt']").InnerText;

//        }

//    }
//    catch
//    {
//        UserID = string.Empty; ;
//    }
//    return UserID;
//}

#endregion