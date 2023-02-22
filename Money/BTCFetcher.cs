using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money
{
    internal class BTCFetcher
    {
        private string _url = String.Empty;
        public BTCFetcher(string url)
        {
            _url = url; 
        }

        public class Currency
        {
            public double _15m { get; set; }
            public double last { get; set; }
            public double buy { get; set; }
            public double sell { get; set; }
            public string symbol { get; set; }
        }

        public class CurrencyCollection : Dictionary<string, Currency>
        {
        }

        public async Task<double> GetBtcPricesAsync()
        {
            string json = await FetchDataAsync();
            CurrencyCollection btcInUsd = JsonConvert.DeserializeObject<CurrencyCollection>(json);
            return btcInUsd["USD"].last;
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
