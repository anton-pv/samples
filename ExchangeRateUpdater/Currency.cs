using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Currency c = obj as Currency;
            if (c == null)
            {
                return false;
            }

            return Code.Equals(c.Code, StringComparison.OrdinalIgnoreCase);
        }
    }
}
