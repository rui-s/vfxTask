namespace ExchangeRates.Interfaces
{
    public interface IExternalAPI
    {
        /// <summary>
        /// Method to read a exchange rate from a external service
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns></returns>
        Task<Models.Exchange?> GetExchangeAsync(string sourceCurrency, string destinationCurrency);
    }
}
