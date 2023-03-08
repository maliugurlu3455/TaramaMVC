using HtmlAgilityPack;
using System.Net;
using System.Text;
using TaramaMVC.Models;

namespace TaramaMVC.Helper
{
    public static class Helperim
    {
        private static readonly IConfiguration configuration;
        private static CookieCollection _ccn =null;
        private static CookieContainer _cocon = null;
        public static CookieContainer Cocon()
        {
            return _cocon;
        }
        public static CookieContainer SetCocon(CookieCollection cc)
        {
            _cocon = new CookieContainer();
            _cocon.Add(cc);
            return _cocon;
        }
        public static CookieCollection ccn()
        {
            return _ccn;    
        }
        public static CookieCollection Setccn(CookieCollection cc)
        {
            _ccn=cc;
            return _ccn;
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
            System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100));
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
                web.UseCookies= true;
                web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.1587.41 Safari/537.36 Edg/110.0.864.52";
                string url = "https://scholar.google.com/citations?hl=tr&pagesize=1000&user=" + users.Trim() + "";
                
                doc = GetPage(url);
                //var responseCookies = web.ResponseCookies;
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
       
        public static List<string> AlintiGetirSelenium(
            //string url
            )
        {
            
            //1f9146a88abddffbb064fbc4e60a22b0f85f0e59068b5e4275b4e454c598c333 
            List<string> liste = null;
            string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
               //"https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ";
           //IWebDriver driver = new ChromeDriver().Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = null;
            HtmlDocument doc2 = null;
       
            doc = GetPage(url);
            var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");
            if (nods != null && nods.Count > 0)
            {
                liste = new List<string>();
                for (int i = 0; i < nods.Count; i++)
                {
                    //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100));
                    var baslikUrl = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");
                   
                    if (baslikUrl.Length>4)
                    {
                        //https://scholar.google.com/scholar?q=info:PEAcbrARXmMJ:scholar.google.com/&output=cite&scirp=0&hl=tr
                        //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
                        string url2 = "https://scholar.google.com/scholar?q=info:"+ baslikUrl + ":scholar.google.com/&output=cite&scirp=0&hl=tr";
                        web = new HtmlWeb();
                  
                        System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
                        doc2 = GetPage(url2);
                        var list1 = doc2.DocumentNode.SelectNodes("//div[@class = 'gs_citr']");
                        
                        if (list1!=null && list1.Count>0)
                        {
                            //for (int j = 0; i < list1.Count; j++)
                            //{
                            liste.Add(list1[1].InnerHtml);
                            //}
                            
                        }
                    }
                    
                }
            }
            return liste;   
        }

        public static List<string> AlintiGetir(string url)
        {
            string _url = url + "&scipsc=&as_ylo="+configuration.GetValue<string>("ylo") +"&as_yhi="+ configuration.GetValue<string>("yhi");
            //1f9146a88abddffbb064fbc4e60a22b0f85f0e59068b5e4275b4e454c598c333 https://scholar.google.com/scholar?as_ylo=2022&hl=tr&as_sdt=2005&sciodt=0,5&cites=4986682506114850678&scipsc=
            List<string> liste = null;
            //string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
            System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = null;
            HtmlDocument doc2 = null;

            //doc = GetPage(url);
            doc = web.Load(_url, "get");
            var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");
            if (nods != null && nods.Count > 0)
            {
                liste = new List<string>();
                for (int i = 0; i < nods.Count; i++)
                {
                    //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100));
                    var baslikUrl = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");

                    if (baslikUrl.Length > 4)
                    {
                        //https://scholar.google.com/scholar?q=info:PEAcbrARXmMJ:scholar.google.com/&output=cite&scirp=0&hl=tr
                        //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
                        string url2 = "https://scholar.google.com/scholar?q=info:" + baslikUrl + ":scholar.google.com/&output=cite&scirp=0&hl=tr";
                                       
                        web = new HtmlWeb();

                        System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
                        //doc2 = GetPage(url2);
                        doc2 = web.Load(url2);
                        var list1 = doc2.DocumentNode.SelectNodes("//div[@class = 'gs_citr']");

                        if (list1 != null && list1.Count > 0)
                        {
                            //for (int j = 0; i < list1.Count; j++)
                            //{
                            liste.Add(list1[1].InnerHtml);
                            //}

                        }
                    }

                }
            }
            return liste;
        }

        public static HtmlDocument GetPage(string url)
        {
            Uri _url=new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            request.ContentType = "text/html; charset=UTF-8";
            if (Cocon == null)
            {
                request.CookieContainer = new CookieContainer();
            }
            else
            {
                request.CookieContainer = Cocon();
            }
            //request.Referer= "";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            Setccn(response.Cookies);
            
            //When you get the response from the website, the cookies will be stored
            //automatically in "_cookies".

            using (var reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-9")))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();
               
               doc.LoadHtml(html);
               
                return doc;
            }
        }
    }

  

}
  //public class MyWebClient
    //{
    //    //The cookies will be here.
    //    private CookieContainer _cookies = new CookieContainer();

    //    //In case you need to clear the cookies
    //    public void ClearCookies()
    //    {
    //        _cookies = new CookieContainer();
    //    }

    //    public HtmlDocument GetPage(string url)
    //    {
    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    //        request.Method = "GET";

    //        //Set more parameters here...
    //        //...

    //        //This is the important part.
    //        request.CookieContainer = _cookies;

    //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //        var stream = response.GetResponseStream();

    //        //When you get the response from the website, the cookies will be stored
    //        //automatically in "_cookies".

    //        using (var reader = new StreamReader(stream))
    //        {
    //            string html = reader.ReadToEnd();
    //            var doc = new HtmlDocument();
    //            doc.LoadHtml(html);
    //            return doc;
    //        }
    //    }
    //}