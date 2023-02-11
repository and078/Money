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
        delegate void TextChangedDelegate(object sender, TextChangedEventArgs e);
        delegate void FocusDelegate(object sender, FocusEventArgs e);

        ChartView chartView;
        ApiFetcher apiFetcher;
        Dictionary<string, List<List<double>>> currencies;
        Dictionary<string, double> todayRates = new Dictionary<string, double>();
        Dictionary<string, Entry> entries;
        Dictionary<string, TextChangedDelegate> textChahgeDelegates;
        Dictionary<string, FocusDelegate> focusDelegates;
        Calculator calculator;
        
        // mdl, usd, ron, rub, uah, gbp, eur
        //DisplayAlert("Changed", "Changed", "Cancel");

        public MainPage()
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                DisplayAlert("No Internet!", "Internet connection needed!", "Abort");
            }
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

            textChahgeDelegates = new Dictionary<string, TextChangedDelegate>()
            {
                { nameof(mdl), Mdl_TextChanged },
                { nameof(usd), Usd_TextChanged },
                { nameof(ron), Ron_TextChanged },
                { nameof(rub), Rub_TextChanged },
                { nameof(uah), Uah_TextChanged },
                { nameof(gbp), Gbp_TextChanged },
                { nameof(eur), Eur_TextChanged },
            };

            focusDelegates = new Dictionary<string, FocusDelegate>()
            {
                { nameof(mdl), Mdl_Focused },
                { nameof(usd), Usd_Focused },
                { nameof(ron), Ron_Focused },
                { nameof(rub), Rub_Focused },
                { nameof(uah), Uah_Focused },
                { nameof(gbp), Gbp_Focused },
                { nameof(eur), Eur_Focused },
            };
            var focus = focusDelegates[nameof(mdl)];

            foreach(var entry in entries)
            {
                var value = entry.Value;
                FocusDelegate focusDelegate = focusDelegates[entry.Key];
                EventHandler<FocusEventArgs> eventHandler = new EventHandler<FocusEventArgs>(focusDelegate);
                value.Focused += eventHandler;
            }

            //mdl.Text = "100";
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
                    Label = new DateTime(1970, 1, 1, 0, 0, 0, 0).Add(TimeSpan.FromMilliseconds((long)rate[0] + DaysInMillisecondsCalculator.MsPerDay)).ToString("dd"),
                    ValueLabel = rate[1] < 0.09 ? 1.0.ToString() : rate[1].ToString(),
                    Color = SKColors.DarkRed,
                    TextColor = SKColors.DarkRed,
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
                LabelTextSize = 18
            };
            chartView = new ChartView { Chart = chart };

            currenciesChart.Children.Add(chartView);
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
                mdlValues[i][1] = 1.0;
            }

            currencies.Add("mdl", mdlValues);

            _ = DrawChart("mdl");
            _ = InitializeTodayRatesAsync();
            calculator = new Calculator();
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

        private void SubscribeTextChangeHandlerBy(string currentCurrency)
        {
            foreach(var entry in entries)
            {
                var value = entry.Value;
                TextChangedDelegate t = textChahgeDelegates[entry.Key];
                EventHandler<TextChangedEventArgs> h = new EventHandler<TextChangedEventArgs>(t);
                value.TextChanged -= h;
            }

            TextChangedDelegate textChangedDelegate = textChahgeDelegates[currentCurrency];
            EventHandler<TextChangedEventArgs> eventHandler = new EventHandler<TextChangedEventArgs>(textChangedDelegate);
            entries[currentCurrency].TextChanged += eventHandler;
        }

        private void Eur_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("eur");
            SubscribeTextChangeHandlerBy("eur");
        }


        private void Eur_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("eur", entries, todayRates);
        }

        private void Gbp_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("gbp");
            SubscribeTextChangeHandlerBy("gbp");
        }

        private void Gbp_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("gbp", entries, todayRates);
        }

        private void Uah_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("uah");
            SubscribeTextChangeHandlerBy("uah");
        }

        private void Uah_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("uah", entries, todayRates);
        }

        private void Rub_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("rub");
            SubscribeTextChangeHandlerBy("rub");
        }

        private void Rub_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("rub", entries, todayRates);
        }

        private void Ron_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("ron");
            SubscribeTextChangeHandlerBy("ron");
        }

        private void Ron_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("ron", entries, todayRates);
        }

        private void Usd_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("usd");
            SubscribeTextChangeHandlerBy("usd");
        }

        private void Usd_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("usd", entries, todayRates);
        }

        private void Mdl_Focused(object sender, FocusEventArgs e)
        {
            _ = DrawChart("mdl");
            SubscribeTextChangeHandlerBy("mdl");
        }

        private void Mdl_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculator.CalculateFor("mdl", entries, todayRates);
        }
    }
}