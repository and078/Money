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
            _ = DrawChart(mdl);

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

        private async Task DrawChart(Entry entry)
        {
            await Task.Delay(1000);
            var charEntries = new List<ChartEntry>();
            var day = DateTime.Now.DayOfWeek;
            var currentCurrency = entry.ToString();
            await DisplayAlert("Changed", currentCurrency.ToString(), "Cancel");

            foreach (var currency in currencies)
            {
                charEntries.Add(new ChartEntry((float)currency.Value[0][1])
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
                Entries = charEntries,
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

        private void Eur_Focused(object sender, FocusEventArgs e)
        {
            DrawChart(eur);
        }

        private void Eur_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Gbp_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Gbp_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Uah_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Uah_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Rub_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Rub_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ron_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ron_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Usd_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Mdl_Focused(object sender, FocusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Mdl_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Usd_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
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

//private void E_Focused(object sender, FocusEventArgs e)
//{
//    if (currencies != null) DisplayAlert("init", "init " + currencies.Count.ToString(), "cancel");
//    //DisplayAlert("Focused", "Currencies ", "Cancel");
//} 

//private void E_TextChanged(object sender, TextChangedEventArgs e)
//{
//    DisplayAlert("Changed", "Changed " + sender.ToString(), "Cancel");
//}

