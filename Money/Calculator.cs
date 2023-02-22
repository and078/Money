using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Money
{ 
    internal class Calculator
    {
        internal void CalculateFor(string currentCurrency, Dictionary<string, Entry> enrties, Dictionary<string, double> todayRates)
        {
            var currentEntry = enrties[currentCurrency];
            var currentTodayRate = todayRates[currentCurrency];
            if(currentEntry.Text == "0")
            {
                currentEntry.Text = String.Empty;
            }

            var currentEntryDouble = currentEntry.Text == String.Empty? 0.0 : Double.Parse(currentEntry.Text);

            var filteredEnrties = enrties.Where(e => e.Key != currentCurrency).ToDictionary(e => e.Key, e => e.Value);
            var filteredTodayRates = todayRates.Where(r => r.Key != currentCurrency).ToDictionary(r => r.Key, r => r.Value);

            foreach(var entry in filteredEnrties)
            {
                var key = entry.Key;
                entry.Value.Text = Math.Round((currentEntryDouble * (currentTodayRate / filteredTodayRates[key])), 6).ToString();
            }
        }
    }
}