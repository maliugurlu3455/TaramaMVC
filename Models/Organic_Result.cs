using GoogleApi.Entities.Maps.Roads.Common;
using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class SearchResult
    {
        public SearchMetadata search_metadata { get; set; }
        public SearchParameters search_parameters { get; set; }
        public SearchInformation search_information { get; set; }
        public Profiles profiles { get; set; }
        public List<OrganicResult> organic_results { get; set; }
    }
    public class SearchResultDetail
    {
        public SearchMetadata search_metadata { get; set; }
        public SearchParameters search_parameters { get; set; }
        public List<citations> citations { get; set; }
        public List<links> links { get; set; }
        
       
    }
    public class SearchMetadata
    {
        public string id { get; set; }
        public string status { get; set; }
        public string json_endpoint { get; set; }
        public DateTime created_at { get; set; }
        public DateTime processed_at { get; set; }
        public string google_scholar_url { get; set; }
        public string raw_html_file { get; set; }
        public double total_time_taken { get; set; }
        public string google_scholar_cite_url { get; set; }
        public string prettify_html_file { get; set; }
       
    }

    public class SearchParameters
    {
        public string engine { get; set; }
        public string cites { get; set; }
        public string hl { get; set; }
        public string num { get; set; }
        public string q { get; set; }
    }

    public class SearchInformation
    {
        public string organic_results_state { get; set; }
        public int total_results { get; set; }
        public double time_taken_displayed { get; set; }
    }

    public class Profiles
    {
        public string link { get; set; }
        public string serpapi_link { get; set; }
    }

    public class OrganicResult
    {
        public int position { get; set; }
        public string title { get; set; }
        public string result_id { get; set; }
        public string link { get; set; }
        public string snippet { get; set; }
        public PublicationInfo publication_info { get; set; }
        public InlineLinks inline_links { get; set; }
    }

    public class PublicationInfo
    {
        public string summary { get; set; }
        public List<Author> authors { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string link { get; set; }
        public string serpapi_scholar_link { get; set; }
        public string author_id { get; set; }
    }

    public class InlineLinks
    {
        public string serpapi_cite_link { get; set; }
        public Versions versions { get; set; }
        // Diğer alanlar da buraya eklenebilir
    }

    public class Versions
    {
        public int total { get; set; }
        public string link { get; set; }
        public string cluster_id { get; set; }
        public string serpapi_scholar_link { get; set; }
    }


    public class citations
    {
        public string title { get; set; }
        public string snippet { get; set; }
    }
    public class links
    {
        public string name { get; set; }
        public string link { get; set; }
        // Diğer alanlar da buraya eklenebilir
    }
}
