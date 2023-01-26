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
        static Dictionary<string, List<List<double>>> currencies;
        public MainPage()
        {
            InitializeComponent();
            DeviceDisplay.KeepScreenOn = true;
            this.BackgroundColor = Color.Black;
            _ = InitializeCurrencies();
            _ = DrawChart();
        }

        private async Task DrawChart()
        {
            await Task.Delay(1000);
            var entries = new List<ChartEntry>();
            foreach (var currency in currencies)
            {
                var day = DateTime.Now.DayOfWeek;
                entries.Add(new ChartEntry((float)currency.Value[0][1])
                {
                    Label = day.ToString(),
                    ValueLabel = currency.Value[0][1].ToString(),
                    Color = SKColors.Gray,
                    TextColor = SKColors.Gray,
                    ValueLabelColor = SKColors.Gray
                });
            }

            var chart = new LineChart
            {
                Entries = entries,
                BackgroundColor = SKColor.Parse("#031b29"),
                LabelTextSize = 30
            };
            chartView = new ChartView { Chart = chart };

            stackLayout.Children.Add(chartView);
        }

        async Task InitializeCurrencies()
        {
            apiFetcher = new ApiFetcher();
            await Task.Run(async () =>
            {
                currencies = await apiFetcher.GetObjectAsync();
            });
        }


        private void E_Focused(object sender, FocusEventArgs e)
        {
            if (currencies != null) DisplayAlert("init", "init " + currencies.Count.ToString(), "cancel");
            //DisplayAlert("Focused", "Currencies ", "Cancel");
        }

        private void E_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayAlert("Changed", "Changed " + sender.ToString(), "Cancel");
        }
    }
}
//
//

//var userValues = new List<Entry> { mdl, usd, ron, rub, uah, gbp, eur };

//userValues.ForEach((e) =>
//{
//    e.TextChanged += E_TextChanged;
//    e.Focused += E_Focused;
//});