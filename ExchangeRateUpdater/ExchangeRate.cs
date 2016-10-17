namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}={2:0.000}", SourceCurrency.Code, TargetCurrency.Code, Value);
        }
    }
}
