using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Money
{ 
    internal class Calculator
    {
        private Dictionary<string, Entry> _enrties;
        private Dictionary<string, double> _todayRates; 
        private string _currentCurrency; 
        public Calculator( string currentCurrency, Dictionary<string, Entry> entries, Dictionary<string, double> todayRates)
        {
            _currentCurrency = currentCurrency;  
            _enrties = entries;
            _todayRates = todayRates;
        }

        internal void CalculateFor(string currentCurrency, Dictionary<string, Entry> enrties, Dictionary<string, double> todayRates)
        {
            var currentEntry = enrties[currentCurrency];
            var currentTodayRate = todayRates[currentCurrency];

            var currentEntryDouble = Double.Parse(currentEntry.Text);

            //var filteredEnrties = enrties.Where(e => e.Key != currentCurrency).ToDictionary(e => e.Key, e => e.Value);
            //var filteredTodayRates = todayRates.Where(r => r.Key != currentCurrency).ToDictionary(r => r.Key, r => r.Value);

            foreach(var entry in enrties)
            {
                var key = entry.Key;
                entry.Value.Text = (currentEntryDouble * (currentTodayRate / todayRates[key])).ToString();
            } 
        }
    } 
}
