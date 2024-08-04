namespace ExchangeRates.Interfaces
{
    /// <summary>
    /// database interface
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// tries to read an exchange for the currency pair
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns>object with data</returns>
        Task<Models.Exchange> ReadExchangeAsync(string sourceCurrency, string destinationCurrency);

        /// <summary>
        /// Creates a Exchange record in database
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns>the created record</returns>
        Task<Models.Exchange> CreateExchangeAsync(Models.Exchange exchange);

        /// <summary>
        /// Updates an Exchange record
        /// </summary>
        /// <param name="exchange"></param>
        Task UpdateExchangeAsync(Models.Exchange exchange);

        /// <summary>
        /// Deletes an exchange record
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        Task DeleteExchangeAsync(string sourceCurrency, string destinationCurrency);
    }
}
