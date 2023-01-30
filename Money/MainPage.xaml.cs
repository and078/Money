using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Money
{
    public partial class MainPage : ContentPage
    {
        ChartView chartView;
        ApiFetcher apiFetcher;
        Dictionary<string, List<List<double>>> currencies;
        Dictionary<string, double> todayRates = new Dictionary<string, double>();
        Dictionary<string, Entry> entries;
        Calculator calculator;

        // mdl, usd, ron, rub, uah, gbp, eur
        //DisplayAlert("Changed", "Changed", "Cancel");

        public MainPage()
        {
            InitializeComponent();
            _ = InitializeCurrenciesAsync();

            DeviceDisplay.KeepScreenOn = true;
            this.BackgroundColor = Color.Black;

            entries = new Dictionary<string, Entry>()
            {
                { nameof(mdl), mdl},
                { nameof(usd), usd},
                { nameof(ron), ron},
                { nameof(rub), rub},
                { nameof(uah), uah},
                { nameof(gbp), gbp},
                { nameof(eur), eur},
            };
            
            calculator = new Calculator(entries, todayRates);


            mdl.Text = "100";

            mdl.TextChanged += Mdl_TextChanged;
            mdl.Focused += Mdl_Focused;

            usd.TextChanged += Usd_TextChanged;
            usd.Focused += Usd_Focused;

            ron.TextChanged += Ron_TextChanged;
            ron.Focused += Ron_Focused;

            rub.TextChanged += Rub_TextChanged;
            rub.Focused += Rub_Focused;

            uah.TextChanged += Uah_TextChanged;
            uah.Focused += Uah_Focused;

            gbp.TextChanged += Gbp_TextChanged;
            gbp.Focused += Gbp_Focused;

            eur.TextChanged += Eur_TextChanged;
            eur.Focused += Eur_Focused;
        }

        private async Task DrawChart(string currentCurrency)
        {
            await Task.Delay(100);
            var charEntries = new List<ChartEntry>();
            var currency = currencies[currentCurrency];
            var rates = currency.SelectMany(arr => arr.Where((x, i) => i % 2 == 1)).ToList();

            foreach (var rate in currency)
            {
                charEntries.Add(new ChartEntry((float)rate[1])
                {
                    Label = new DateTime(1970, 1, 1, 0, 0, 0, 0).Add(TimeSpan.FromMilliseconds((long)rate[0])).ToString("yyyy-MM-dd"),
                    ValueLabel = rate[1].ToString(),
                    Color = SKColors.Gray,
                    TextColor = SKColors.Gray,
                    ValueLabelColor = SKColors.Gray
                });
            }

            var chart = new LineChart
            {
                Entries = charEntries,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation= Orientation.Horizontal,
                MaxValue = (float)rates.Max(),
                MinValue = (float)rates.Min(),
                BackgroundColor = SKColor.Parse("#031b29"),
                LabelTextSize = 23
            };
            chartView = new ChartView { Chart = chart };

            stackLayout.Children.Add(chartView);
        }

        async Task InitializeCurrenciesAsync()
        {
            apiFetcher = new ApiFetcher();
            await Task.Run(async () =>
            {
                currencies = await apiFetcher.GetObjectAsync();
            });
            currencies.Add("mdl", );

            _ = DrawChart("usd");
            _ = InitializeTodayRatesAsync();
        }

        async Task InitializeTodayRatesAsync()
        {
            await Task.Delay(100);
            if(currencies != null)
            {
                foreach (var currency in currencies)
                {
                    todayRates.Add(currency.Key, currency.Value[6][1]);
                }
            }
        }

        private void Eur_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("eur");
        }

        private void Eur_TextChanged(object sender, TextChangedEventArgs e)
        {
            eur.Text = todayRates["eur"].ToString();
        }

        private void Gbp_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("gbp");
        }

        private void Gbp_TextChanged(object sender, TextChangedEventArgs e)
        {
            gbp.Text = todayRates["gbp"].ToString();
        }

        private void Uah_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("uah");
        }

        private void Uah_TextChanged(object sender, TextChangedEventArgs e)
        {
            uah.Text = todayRates["uah"].ToString();
        }

        private void Rub_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("rub");
        }

        private void Rub_TextChanged(object sender, TextChangedEventArgs e)
        {
            rub.Text = todayRates["rub"].ToString();
        }

        private void Ron_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("ron");
        }

        private void Ron_TextChanged(object sender, TextChangedEventArgs e)
        {
            ron.Text = todayRates["ron"].ToString();
        }

        private void Usd_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("usd");
        }

        private void Usd_TextChanged(object sender, TextChangedEventArgs e)
        {
            usd.Text = todayRates["usd"].ToString();
        }

        private void Mdl_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("mdl");
        }

        private void Mdl_TextChanged(object sender, TextChangedEventArgs e)
        {
            mdl.Text = todayRates["mdl"].ToString();
        }
    }
}