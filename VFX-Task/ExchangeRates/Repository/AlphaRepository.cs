using ExchangeRates.Interfaces;
using ExchangeRates.Models;
using System.Text.Json;

namespace ExchangeRates.Repository
{
    public class AlphaRepository : IExternalAPI
    {
        private readonly string apiUrl;
        private readonly string apiKey;
        private readonly ILogger<AlphaRepository> logger;

        /// <summary>
        /// Alpha Vantage repository contructor
        /// </summary>
        /// <param name="configuration"></param>
        public AlphaRepository(IConfiguration configuration, ILogger<AlphaRepository> logger)
        {
            // read settings
            apiUrl = configuration.GetValue<string>("ExternalAPI:url");
            apiKey = configuration.GetValue<string>("ExternalAPI:key");
            this.logger = logger;   
        }

        public async Task<Exchange?> GetExchangeAsync(string sourceCurrency, string destinationCurrency)
        {
            Exchange result = null;

            using (var client = new HttpClient())
            {
                // build uri request
                string request = $"{apiUrl}/query?function=CURRENCY_EXCHANGE_RATE&from_currency={sourceCurrency}&to_currency={destinationCurrency}&apikey={apiKey}";

                logger.LogInformation("Request to: {0}", request);

                // make request
                var httpResponse = await client.GetAsync(request);

                if (httpResponse?.IsSuccessStatusCode == true)
                {
                    var jsonResult = await httpResponse.Content.ReadAsStringAsync();

                    logger.LogDebug("Response from API: {0}", jsonResult);

                    // check if error message is retrive
                    if (jsonResult.Contains("Error Message"))
                    {
                        return null;
                    }

                    // deserialize for a generic type
                    var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonResult);

                    if (jsonData.ContainsKey("Realtime Currency Exchange Rate"))
                    {
                        // read values from response
                        var jsonProperties = jsonData["Realtime Currency Exchange Rate"];

                        var srcCur = jsonProperties["1. From_Currency Code"];
                        var dstCur = jsonProperties["3. To_Currency Code"];

                        // confirm values are ok
                        if (srcCur != null && dstCur != null 
                            && decimal.TryParse(jsonProperties["8. Bid Price"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal bid) 
                            && decimal.TryParse(jsonProperties["9. Ask Price"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var ask))
                        {
                            result = new Exchange
                            {
                                SourceCurrency = srcCur,
                                DestinationCurrency = dstCur,
                                AskValue = ask,
                                BidValue = bid
                            };
                        }
                    }
                }
            }

            return result;
        }
    }
}
