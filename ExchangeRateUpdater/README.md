# Anton Pereimybida

This is first draft implementation of the task.


Here task explanation i've got from recruiter:

> [https://github.com/MewsSystems/samples/tree/master/ExchangeRateUpdater](https://github.com/MewsSystems/samples/tree/master/ExchangeRateUpdater)
>
>"It is a fairly simple task - ExchangeRateProvider obtains data from the Norwegian National Bank, so please implement this using the samples provided to you."

I have two questions regardless the task: 

1. **How is data obtained?** My guess was to obtain data from Norwegian National Bank in two ways - some open API or file (on their website). Couldn't find any API, only
.csv files availabel for download. 
But that file was holding rates only for NOK, so it seems too simple for the ExchangeRateprovider. And that's why I also added a generator for pairs, just to provide more interesting data.

## Note: please copy input.csv file into application folder.

2. **What is the format of source?** Next, I looked into ***ExchangeRateProvider*** and its comment:

> Should return exchange rates among the specified currencies that are defined by the source. But only those defined
> by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
> do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
> some of the currencies, ignore them.

(and this format doesn't match .csv file from website)