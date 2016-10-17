using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public enum SourceMode
    {
        Generate,
        File,
        Server
    }

    public class ExchangeRateProvider
    {
        #region Fields

        private ExchangeRate[] m_sourceRates;

        #endregion

        #region Constructors

        public ExchangeRateProvider(SourceMode mode)
        {
            switch (mode)
            {
                case SourceMode.Generate:
                    GenerateSourceRates();
                    break;

                case SourceMode.File:
                    LoadSourceRatesFromFile();
                    break;

                case SourceMode.Server:
                    RetrieveSourceRatesFromServer();
                    break;

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        #endregion

        #region Fill source

        private void GenerateSourceRates()
        {
            string[] currencies = new string[] { "USD", "EUR", "CZK", "UAH", "RUR", "JPY" };
            
            List<ExchangeRate> result = new List<ExchangeRate>();
            Random r = new Random();

            foreach (string sourceCurrency in currencies)
            {
                int numberOfPairs = r.Next(currencies.Length); //take random number of target currency pairs for our source currency

                List<string> targetCurrencies = new List<string>(currencies);
                targetCurrencies.Remove(sourceCurrency); // avoid USD/USD rates
                
                for (int i = 0; i < numberOfPairs; i++)
                {
                    int position = r.Next(targetCurrencies.Count - 1);
                    string targetCurrency = targetCurrencies[position]; // retrieve random target currency

                    decimal rate = (decimal)r.NextDouble() * 100;

                    result.Add(new ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), rate));

                    targetCurrencies.Remove(targetCurrency); //avoid duplicates
                }
            }
            
            m_sourceRates = result.ToArray();
        }

        private void LoadSourceRatesFromFile()
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            using (TextFieldParser parser = new TextFieldParser("input.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                //we take only header row and one value row 
                string[] headers = parser.ReadFields();
                string[] values = parser.ReadFields();

                for (int i = 0; i < headers.Length; i++)
                {
                    #region Parse header

                    string header = headers[i];

                    string[] h = header.Split(' ');

                    int multiplier; //defines amount of source currency in provide exchange rate

                    if (h.Length != 2 || 
                        !int.TryParse(h[0], out multiplier) || 
                        h[1].Length != 3) //apply simple check - currency should have three letters
                    {
                        continue;
                    }

                    string targetCurrency = h[1];

                    #endregion

                    #region Parse value

                    string value = values[i];

                    decimal rate;
                    if (!decimal.TryParse(value, out rate))
                    {
                        continue;
                    }

                    rate /= multiplier;

                    #endregion

                    result.Add(new ExchangeRate(new Currency(targetCurrency), new Currency("NOK"), rate));
                }
            }

            m_sourceRates = result.ToArray();
        }

        private void RetrieveSourceRatesFromServer()
        {
            
        }

        #endregion

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> result = m_sourceRates.Where(
                r => (currencies.Contains(r.SourceCurrency) && currencies.Contains(r.TargetCurrency)));

            return result;
        }
    }
}
