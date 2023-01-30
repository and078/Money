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
        public Calculator(Dictionary<string, Entry> entries, Dictionary<string, double> todayRates)
        {
            _enrties = entries;
            _todayRates = todayRates;
        }

        internal string CalculateFor(string currency)
        {

            return currency;
        }
    }
}
