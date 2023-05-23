using SerpApi;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;
using System.Text;
using TaramaMVC.Models;
using GoogleApi.Entities.Search.Video.Common;
using Newtonsoft.Json;
using NuGet.Configuration;

namespace TaramaMVC.Helper
{
    public static class Helperim
    {
        
        private static CookieCollection _ccn = null;
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
            _ccn = cc;
            return _ccn;
        }
        public static string[] GetUserId(string AdSoyad, bool hepsi)
        {
            HtmlDocument doc = null;
            string[] UserID = new string[] { string.Empty, string.Empty, string.Empty };
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
                    string value = nod.SelectSingleNode("//div[@class='gs_ai_cby']").InnerText.Replace("Alıntılanma sayısı: ", "").Trim();
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
            System.Threading.Thread.Sleep(Random.Shared.Next(20000, 30000));
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
                web.UseCookies = true;
                web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
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
                    if (alintitoplamnod != null)
                    {
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
                        if (i == nods.Count - 1)
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

                            baslik = "";
                        }
                        try
                        {
                            baslikcites = nods[i].SelectNodes("//a[@class='gsc_a_ac gs_ibl']")[i].GetAttributeValue("href", "");
                            if (baslikcites != "" && baslikcites != " ")
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
                            if (alintilanma == "")
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
            catch (Exception ex)
            {
                string hata = baslikcites + " - " + ex.Message;
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

                    if (baslikUrl.Length > 4)
                    {
                        //https://scholar.google.com/scholar?q=info:PEAcbrARXmMJ:scholar.google.com/&output=cite&scirp=0&hl=tr
                        //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
                        string url2 = "https://scholar.google.com/scholar?q=info:" + baslikUrl + ":scholar.google.com/&output=cite&scirp=0&hl=tr";
                        web = new HtmlWeb();

                        System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
                        doc2 = GetPage(url2);
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

        public static List<string> AlintiGetir(string url)//, string ylo, string yhi)
        {
            string _url = url;// + "&scipsc=&as_ylo="+ ylo  +"&as_yhi="+  yhi;
            //1f9146a88abddffbb064fbc4e60a22b0f85f0e59068b5e4275b4e454c598c333 https://scholar.google.com/scholar?as_ylo=2022&hl=tr&as_sdt=2005&sciodt=0,5&cites=4986682506114850678&scipsc=
            List<string> liste = null;
            //string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
            //https://scholar.google.com/scholar?output=cite&q=info:EtmvpemobM0J:scholar.google.com
            System.Threading.Thread.Sleep(Random.Shared.Next(10000, 15000));
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            HtmlDocument doc = null;
            HtmlDocument doc2 = null;

            //doc = GetPage(url);
            doc = web.Load(_url, "get");
            var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");
            if (nods != null && nods.Count > 0)
            {
                liste = new List<string>();
                int sayac = 0;
                for (int i = 0; i < nods.Count; i++)
                {
                    //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100));
                    var baslikUrl = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");

                    if (baslikUrl.Length > 4)
                    {
                        //https://scholar.google.com/scholar?q=info:PEAcbrARXmMJ:scholar.google.com/&output=cite&scirp=0&hl=tr
                        //https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ
                        //https://scholar.google.com/scholar?output=cite&q=info:EtmvpemobM0J:scholar.google.com
                        string url2 = "https://scholar.google.com/scholar?q=info:" + baslikUrl + ":scholar.google.com/&output=cite&scirp=" + sayac + "&hl=tr";

                        web = new HtmlWeb();
                        web.UseCookies = true;
                        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";

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
        public static List<string> AlintiGetirWithApi(string adres, string ApiKey)//, string ylo, string yhi)
        {
            List<string> liste = null;
            /*
            string _url = adres;// + "&scipsc=&as_ylo="+ ylo  +"&as_yhi="+  yhi;
            //1f9146a88abddffbb064fbc4e60a22b0f85f0e59068b5e4275b4e454c598c333 https://scholar.google.com/scholar?as_ylo=2022&hl=tr&as_sdt=2005&sciodt=0,5&cites=4986682506114850678&scipsc=
           
            string endpoint = "https://serpapi.com/search.json";

            // API anahtarınızı buraya girin.
            string apiKey = "YOUR_API_KEY";

            // Alıntılar için arama sorgusu oluşturun.
            string query = $"cites:{1596913064683209329}";
            string paramsString = $"q={query}&hl=en&api_key={apiKey}";

            // HTTP GET isteği oluşturun.
            HttpClient client = new HttpClient();
            Uri uri = new Uri(endpoint + "?" + paramsString);
            HttpResponseMessage response =await client.GetAsync(uri);

            // İsteğin başarılı olduğunu kontrol edin.
            if (response.IsSuccessStatusCode)
            {
                // İsteğin yanıtını JSON olarak ayrıştırın.
                JObject responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                // Alıntılar için URL'leri listelemek için JSON yanıtını işleyin.
                JArray items = (JArray)responseJson["items"];
                foreach (JObject item in items)
                {
                    Console.WriteLine(item["link"]);
                }
            }
            return liste;
            */
            liste = new List<string>();
            return liste;
        }
        public static HtmlDocument GetPage(string url)
        {
            Uri _url = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            request.ContentType = "text/html; charset=UTF-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";


            if (Cocon == null)
            {

                request.CookieContainer = new CookieContainer();
            }
            else
            {
                request.CookieContainer = new CookieContainer();

            }


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            Setccn(response.Cookies);
            using (var reader = new StreamReader(stream, Encoding.UTF8))//Encoding.GetEncoding("iso-8859-9")))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();

                doc.LoadHtml(html);

                return doc;
            }
        }
        public static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss 'UTC'"
        };
        public static List<YayinAlintiBilgisi> GetSearchResultCitedID(string ApiKey, string CiteID,PersonelYayinBilgileri pyb)
        {
            List<YayinAlintiBilgisi> liste = null; 
            GoogleSearch search = null;
                        string apiKey = ApiKey;
                        Hashtable ht = new Hashtable();
                        ht.Add("engine", "google_scholar");//ht.Add("engine", "google_scholar_cite");
                        ht.Add("hl", "tr");
                        ht.Add("num", "20");
                        ht.Add("as_ylo", string.Format("{0}",DateTime.Now.Year-1));
                        ht.Add("cites", CiteID);

                        try
                        {
                            search = new GoogleSearch(ht, apiKey);

                            JObject data = search.GetJson();
                            if (search != null)
                            {

                    SearchResult res = JsonConvert.DeserializeObject<SearchResult>(data.ToString(), settings);
                    if (res != null)
                    {
                        if (res.search_metadata.status == "Success")
                        {
                            YayinAlintiBilgisi yab = null;
                            liste = new List<YayinAlintiBilgisi>();
                            foreach (OrganicResult item in res.organic_results)
                            {
                                yab = new YayinAlintiBilgisi();
                                yab.Title = item.title;
                                yab.Link = item.link;
                                yab.SID = item.result_id;
                                yab.PublicationInfo = item.publication_info.summary;
                                yab.Snippet = item.snippet;
                                yab.personelYayinBilgileriId = pyb.Id;
                                yab.personelYayinBilgileri = pyb;

                                //                    yab.Ad = gs_ri1;
                                yab.Tip = "APA";
                                yab.status = 0;
                                liste.Add(yab);
                                //await _context.YayinAlintiBilgisis.AddAsync(yab);
                                //await _context.SaveChangesAsync();

                                //item.title
                                //item.snippet
                                //item.position
                                //item.inline_links
                                //item.link
                                //item.publication_info
                                //item.result_id

                            }
                        }
                    }
                }
                        }
            catch (SerpApiSearchException ex)
            {
                liste = null;
            }
            return liste;
        }

      
        public static async Task<HtmlDocument> GetPageYeni(string url)
        { 
            string _url = url;// "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "text/html; charset=UTF-8");
            client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
            //request.Method = "GET";
            //request.ContentType = "text/html; charset=UTF-8";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            string responseBody = await client.GetStringAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseBody);
            return doc;
        }
        public static string UrltoCites(string Url)
        {
            string cites = "";
            if (!string.IsNullOrEmpty(Url))
            {
               string[] cite = Url.Split("cites=");
                if (cite.Length>1)
                {
                    cites = cite[1];
                }
            }
            return cites;
        }
        //https://scholar.google.com/scholar?q=info:t1YVpgWg9h4J:scholar.google.com/&output=cite&hl=tr
        static HtmlWeb web = new HtmlWeb() {AutoDetectEncoding=true,UseCookies=true,UserAgent= "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36" };
        public static List<string> AlintiAyrintiGetir(string ID)//, string ylo, string yhi)
        {
           
            List<string> liste =null;
            
            System.Threading.Thread.Sleep(Random.Shared.Next(10000, 15000));
            //HtmlWeb web = new HtmlWeb();
            //web.UseCookies = true;
             //web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
            HtmlDocument doc = null;

          
                        string url = "https://scholar.google.com/scholar?q=info:" + ID + ":scholar.google.com/&output=cite&hl=tr";

                       
                       /// web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";

                        //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 6100));
                        //doc2 = GetPage(url2);
                        doc = web.Load(url);
                        var list1 = doc.DocumentNode.SelectNodes("//div[@class = 'gs_citr']");

                        if (list1 != null && list1.Count > 0)
                        {
                            //for (int j = 0; i < list1.Count; j++)
                            //{
                            liste.Add(list1[1].InnerHtml);
                //}

            }
            else
            {
                liste = new List<string>(){""};
            }

                
            
            return liste;
        }
    }
}