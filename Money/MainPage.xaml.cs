using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
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

            apiFetcher = new ApiFetcher();

            currencies = apiFetcher.GetObjectAsync().Result;
            DisplayAlert("init", "init " + currencies.Count.ToString(), "cancel");
            

            var userValues = new List<Entry> { mdl, usd, ron, rub, uah, gbp, eur };

            userValues.ForEach((e) =>
            {
                e.TextChanged += E_TextChanged;
                e.Focused += E_Focused;
            });

            var entries = new List<ChartEntry>();

            for (int i = 0; i < 7; i++)
            {
                entries.Add(new ChartEntry(5.6F)
                {
                    Label = i.ToString(),
                    ValueLabel = i.ToString(),
                    Color = SKColors.Gray,
                    TextColor = SKColors.Gray,
                    ValueLabelColor = SKColors.Gray
                }); 
            }
            var chart = new LineChart { Entries = entries, BackgroundColor = SKColor.Parse("#031b29") };
            chart.LabelTextSize = 30;
            chartView = new ChartView { Chart = chart };

            stackLayout.Children.Add(chartView);
        }


        //async void InitializeCurrencies()
        //{
        //    ApiFetcher fetcher = new ApiFetcher();
        //    await Task.Run(() =>
        //    {
        //        currencies = fetcher.GetFetched();                
        //    });
        //}


        private void E_Focused(object sender, FocusEventArgs e)
        {
            DisplayAlert("Focused", "Currencies ", "Cancel");
        }

        private void E_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayAlert("Changed", "Changed " + sender.ToString(), "Cancel");
        }
    }
}
/*

var userValues = new List<Entry>();
userValues.Add(mdl);
userValues.Add(usd);
userValues.Add(ron);
userValues.Add(rub);
userValues.Add(uah);
userValues.Add(gbp);
userValues.Add(eur);

userValues.ForEach((e) =>
{
    e.TextChanged += E_TextChanged;
    e.Focused += E_Focused;
});

var entries = new List<ChartEntry>();

for (int i = 0; i < 7; i++)
{
    entries.Add(new ChartEntry(5.6F)
    {
        Label = i.ToString(),
        ValueLabel = i.ToString(),
        Color = SKColors.Gray,
        TextColor = SKColors.Gray,
        ValueLabelColor = SKColors.Gray
    });
}
var chart = new LineChart { Entries = entries, BackgroundColor = SKColor.Parse("#031b29") };
chart.LabelTextSize = 30;
chartView = new ChartView { Chart = chart };

stackLayout.Children.Add(chartView);
//currencies = fetcher.GetFetched();
DisplayAlert("Internet", $"{Connectivity.NetworkAccess}", "Cancel");

*/