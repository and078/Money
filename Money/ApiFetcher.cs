using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money
{
    internal class ApiFetcher
    {
        private string _url = string.Empty;
        public ApiFetcher(string url)
        {
            _url = url;
        }

        public async Task<Dictionary<string, List<List<double>>>> GetObjectAsync()
        {
            string json = await FetchDataAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, List<List<double>>>>(json);       
        }

        private async Task<string> FetchDataAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetAsync(_url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
