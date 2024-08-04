using ExchangeRates.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Rec = ExchangeRates.Models.Exchange;

namespace ExchangeRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Exchange : ControllerBase
    {
        private readonly IDatabase database;
        private readonly IExternalAPI api;
        private readonly ILogger<Exchange> logger;

        /// <summary>
        /// controller contructor
        /// </summary>
        /// <param name="db">repository from injection</param>
        /// <param name="api">repository from injection</param>
        /// <param name="logger">logger interface</param>
        public Exchange(IDatabase db, IExternalAPI api, ILogger<Exchange> logger)
        {
            this.database = db;
            this.api = api;
            this.logger = logger;
        }


        // GET: api/<Exchange>
        [HttpGet("{sourceCurrency}/{destinationCurrency}")]
        public async Task<ActionResult<Rec?>> GetAsync([FromRoute]string sourceCurrency, [FromRoute]string destinationCurrency)
        {
            try
            {
                // try to read value from database
                var result = await database.ReadExchangeAsync(sourceCurrency, destinationCurrency);

                if (result == null)
                {
                    // if no results read from external API
                    var newRecord = await api.GetExchangeAsync(sourceCurrency, destinationCurrency);

                    // if sucess from external api insert into database
                    if (newRecord != null)
                    {
                        // insert value in database
                        return Ok(await database.CreateExchangeAsync(newRecord));
                    }
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (SqlException dataError)
            {
                logger.LogError("Error reading record: {@err}", dataError);
                return BadRequest($"error in database! > {dataError.Message}");
            }
            catch (Exception anyError)
            {
                logger.LogError("Error reading record: {@err}", anyError);
                return BadRequest("unexpected error!");
            }


            // if no result return 404
            return NotFound();
        }

        // POST api/<Exchange>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Rec value)
        {
            // validate input information
            if (!Validate(value))
            {
                return BadRequest("invalid input!");
            }

            try
            {
                // create new record
                _ = await database.CreateExchangeAsync(value);
            }
            catch (SqlException dataError)
            {
                logger.LogError("Error creating record: {@err}", dataError);
                return BadRequest($"error in database! > {dataError.Message}");
            }
            catch (Exception anyError)
            {
                logger.LogError("Error reading record: {@err}", anyError);
                return BadRequest("unexpected error!");
            }

            return Ok();
        }

        // PUT api/<Exchange>/{sourceCurrency}/{destinationCurrency}
        [HttpPut("{sourceCurrency}/{destinationCurrency}")]
        public async Task<ActionResult> PutAsync([FromRoute] string sourceCurrency, [FromRoute] string destinationCurrency, [FromBody] Rec value)
        {
            // validate input
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // confirm value into record to be updated
            value.SourceCurrency = sourceCurrency;
            value.DestinationCurrency = destinationCurrency;

            // validate input information
            if (!Validate(value))
            {
                return BadRequest("invalid input!");
            }

            try
            {
                // update record
                await database.UpdateExchangeAsync(value);
            }
            catch (SqlException dataError)
            {
                logger.LogError("Error updating record: {@err}", dataError);
                return BadRequest($"error in database! > {dataError.Message}");
            }
            catch (Exception anyError)
            {
                logger.LogError("Error updating record: {@err}", anyError);
                return BadRequest("unexpected error!");
            }

            return Ok();
        }

        // DELETE api/<Exchange>/{sourceCurrency}/{destinationCurrency}
        [HttpDelete("{sourceCurrency}/{destinationCurrency}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string sourceCurrency, [FromRoute] string destinationCurrency)
        {
            try
            {
                // delete record
                await database.DeleteExchangeAsync(sourceCurrency, destinationCurrency);
            }
            catch (SqlException dataError)
            {
                logger.LogError("Error deleting record: {@err}", dataError);
                return BadRequest($"error in database! > {dataError.Message}");
            }
            catch (Exception anyError)
            {
                logger.LogError("Error deleting record: {@err}", anyError);
                return BadRequest("unexpected error!");
            }

            return Ok();
        }

        private bool Validate(Rec value)
        {
            if (!ModelState.IsValid)
            {
                var errors = value.Validate(new ValidationContext(value, null, null));
                foreach (var error in errors)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }
                }
                return false;
            }

            return true;
        }
    }
}
