using System.ComponentModel.DataAnnotations;

namespace ExchangeRates.Models
{
    public class Exchange : IValidatableObject
    {
        /// <summary>
        /// source currency code
        /// </summary>
        [Required]
        public string SourceCurrency { get; set; }

        /// <summary>
        /// destination currency code
        /// </summary>
        [Required]
        public string DestinationCurrency { get; set; }

        /// <summary>
        /// bid value
        /// </summary>
        [Required]
        public decimal BidValue { get; set; }

        /// <summary>
        /// ask value
        /// </summary>
        [Required]
        public decimal AskValue { get; set; }

        /// <summary>
        /// method to validate object
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(SourceCurrency,
                new ValidationContext(this, null, null) { MemberName = nameof(SourceCurrency) },
                results);
            Validator.TryValidateProperty(DestinationCurrency,
                new ValidationContext(this, null, null) { MemberName = nameof(DestinationCurrency) },
                results);

            Validator.TryValidateProperty(BidValue,
                new ValidationContext(this, null, null) { MemberName = nameof(BidValue) },
                results);
            Validator.TryValidateProperty(AskValue,
                new ValidationContext(this, null, null) { MemberName = nameof(AskValue) },
                results);

            return results;
        }
    }
}
