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
                { nameof(mdl), mdl },
                { nameof(usd), usd },
                { nameof(ron), ron },
                { nameof(rub), rub },
                { nameof(uah), uah },
                { nameof(gbp), gbp },
                { nameof(eur), eur },
            };

            mdl.Text = "100";
            calculator = new Calculator("mdl", entries, todayRates);

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
            await Task.Delay(0);
            var charEntries = new List<ChartEntry>();
            var currency = currencies[currentCurrency];
            var rates = currency.SelectMany(arr => arr.Where((x, i) => i % 2 == 1)).ToList();

            foreach (var rate in currency)
            {
                charEntries.Add(new ChartEntry((float)rate[1])
                {
                    Label = new DateTime(1970, 1, 1, 0, 0, 0, 0).Add(TimeSpan.FromMilliseconds((long)rate[0] + DaysInMillisecondsCalculator.MsPerDay)).ToString("yyyy-MM-dd"),
                    ValueLabel = rate[1] < 0.09 ? 1.0.ToString() : rate[1].ToString(),
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

            var mdlValues = new List<List<double>>();

            foreach (List<double> value in currencies["usd"])
            {
                mdlValues.Add(new List<double>(value));
            }

            for (int i = 0; i < mdlValues.Count; i++)
            {
                var mid = currencies["usd"][i][1];
                mdlValues[i][1] = 1.0 / mid;
            }

            currencies.Add("mdl", mdlValues);

            _ = DrawChart("mdl");
            _ = InitializeTodayRatesAsync();
        }

        async Task InitializeTodayRatesAsync()
        {
            await Task.Delay(0);
            if(currencies != null)
            {
                foreach (var currency in currencies)
                {
                    todayRates.Add(currency.Key, currency.Value[6][1]);
                }
                todayRates.Add("mdl", 1.0);
            }
        }

        private void Eur_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("eur");
        }

        private void Eur_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("eur", entries, todayRates);
        }

        private void Gbp_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("gbp");
        }

        private void Gbp_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("gbp", entries, todayRates);
        }

        private void Uah_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("uah");
        }

        private void Uah_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("uah", entries, todayRates);
        }

        private void Rub_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("rub");
        }

        private void Rub_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("rub", entries, todayRates);
        }

        private void Ron_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("ron");
        }

        private void Ron_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("ron", entries, todayRates);
        }

        private void Usd_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("usd");
        }

        private void Usd_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("usd", entries, todayRates);
        }

        private void Mdl_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("mdl");
        }

        private void Mdl_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("mdl", entries, todayRates);
        }
    }
}