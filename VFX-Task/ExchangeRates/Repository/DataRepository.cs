using Dapper;
using ExchangeRates.Interfaces;
using ExchangeRates.Models;
using Microsoft.Data.SqlClient;

namespace ExchangeRates.Repository
{
    public class DataRepository : IDatabase
    {
        private readonly string DBConnection;

        /// <summary>
        /// database repository constructor
        /// </summary>
        /// <param name="configuration"></param>
        public DataRepository(IConfiguration configuration)
        {
            DBConnection = configuration.GetValue<string>("ConnectionStrings:exchangeDB");
        }

        /// <summary>
        /// create action (insert)
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public async Task<Exchange> CreateExchangeAsync(Exchange exchange)
        {
            // prepare connection
            using (var connection = new SqlConnection(DBConnection))
            {
                // build procedure parameters
                var parameters = new DynamicParameters();
                parameters.Add("sourceCurrency", exchange.SourceCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);
                parameters.Add("destinationCurrency", exchange.DestinationCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);
                parameters.Add("bidValue", exchange.BidValue, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                parameters.Add("askValue", exchange.AskValue, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);

                // execute stored procedure
                await connection.ExecuteAsync("dbo.CreateExchange", param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return await ReadExchangeAsync(exchange.SourceCurrency, exchange.DestinationCurrency);
        }

        /// <summary>
        /// delete action
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns></returns>
        public async Task DeleteExchangeAsync(string sourceCurrency, string destinationCurrency)
        {
            // prepare connection
            using (var connection = new SqlConnection(DBConnection))
            {
                // build procedure parameters
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("sourceCurrency", sourceCurrency),
                    new SqlParameter("destinationCurrency", destinationCurrency)
                };

                // prepare command for execution
                var command = new SqlCommand("dbo.DeleteExchange", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                // open connection
                await connection.OpenAsync();
                // execute stored procedure
                await command.ExecuteNonQueryAsync();
                // close connection
                await connection.CloseAsync();
            }
        }

        /// <summary>
        /// read action
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns></returns>
        public async Task<Exchange> ReadExchangeAsync(string sourceCurrency, string destinationCurrency)
        {
            // prepare connection
            using (var connection = new SqlConnection(DBConnection))
            {
                // build procedure parameters
                var parameters = new DynamicParameters();
                parameters.Add("sourceCurrency", sourceCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);
                parameters.Add("destinationCurrency", destinationCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);

                // execute stored procedure
                return await connection.QueryFirstOrDefaultAsync<Exchange>("dbo.GetExchange", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// update action
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public async Task UpdateExchangeAsync(Exchange exchange)
        {
            // prepare connection
            using (var connection = new SqlConnection(DBConnection))
            {
                // build procedure parameters
                var parameters = new DynamicParameters();
                parameters.Add("sourceCurrency", exchange.SourceCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);
                parameters.Add("destinationCurrency", exchange.DestinationCurrency, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 10);
                parameters.Add("bidValue", exchange.BidValue, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                parameters.Add("askValue", exchange.AskValue, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);

                // execute stored procedure
                await connection.ExecuteAsync("dbo.UpdateExchange", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
