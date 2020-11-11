using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GithubApi
{
    public class WebCaller
    {
        public async Task<List<Repos>> FetchRepoAsync(string organisation)
        {
            string result = null;
            var res = new List<Repos>();
            int pageNo = 0;
            int cnt = 5;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            
                //string cred = $"Basic {Base64Encode($"{Credentials.Username}:{Credentials.Password}")}";
                //client.DefaultRequestHeaders.Add("Authorization", cred);

                var byteArray = Encoding.ASCII.GetBytes($"{Credentials.Username}:{Credentials.Password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                
                //while (string.IsNullOrEmpty(result) == false)
                while (cnt-- > 0)
                {
                    pageNo++;
                    var url = string.Format("https://api.github.com/orgs/{0}/repos?page={1}&per_page=100", organisation, pageNo);
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    using (HttpContent content = response.Content)
                    {
                        result = await content.ReadAsStringAsync();
                        res.AddRange(JsonConvert.DeserializeObject<List<Repos>>(result));
                    }
                }
            }

            return res;
        }

        public async Task<List<Contributors>> FetchTopContributorsAsync(string organisation, string RepoName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            int pageNo = 0;
            int cnt = 5;
            string result = null;
            var res = new List<Contributors>();


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

                var byteArray = Encoding.ASCII.GetBytes($"{Credentials.Username}:{Credentials.Password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //while (string.IsNullOrEmpty(result) == false)
                while (cnt-- > 0)
                {
                    pageNo++;
                    var url = string.Format("https://api.github.com/repos/{0}/{1}/contributors?q=contributions&order=desc?page={2}&per_page=100", organisation,RepoName,pageNo);
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    using (HttpContent content = response.Content)
                    {
                        result = await content.ReadAsStringAsync();
                        res.AddRange(JsonConvert.DeserializeObject<List<Contributors>>(result));
                    }
                }
            }

            return res;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
