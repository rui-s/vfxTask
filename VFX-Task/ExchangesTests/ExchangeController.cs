using ExchangeRates.Controllers;
using ExchangeRates.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangesTests
{
    public class ExchangeController
    {
        [Fact]
        public async Task Get_FromDBAsync()
        {
            // arrange
            var mockedDB = new Mock<IDatabase>();
            var mockExternalAPI = new Mock<IExternalAPI>();
            var mockedLogger = new Mock<ILogger<Exchange>>();

            var expectedResult = new ExchangeRates.Models.Exchange
            {
                SourceCurrency = "eur",
                DestinationCurrency = "usd",
                BidValue = 1.002M,
                AskValue = 0.9988M
            };

            mockedDB.Setup(db => db.ReadExchangeAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency))
                .ReturnsAsync(expectedResult);

            var testedCode = new Exchange(mockedDB.Object, mockExternalAPI.Object, mockedLogger.Object);

            // act
            var result = await testedCode.GetAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency);

            // assert 
            Assert.NotNull(result?.Result);
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultTestValue = Assert.IsType<ExchangeRates.Models.Exchange>(actionResult.Value);
            Assert.Equal(expectedResult, resultTestValue);
        }

        [Fact]
        public async Task Get_FromExternalAPIAsync()
        {
            // arrange
            var mockedDB = new Mock<IDatabase>();
            var mockExternalAPI = new Mock<IExternalAPI>();
            var mockedLogger = new Mock<ILogger<Exchange>>();

            var expectedResult = new ExchangeRates.Models.Exchange
            {
                SourceCurrency = "eur",
                DestinationCurrency = "usd",
                BidValue = 1.002M,
                AskValue = 0.9988M
            };
            ExchangeRates.Models.Exchange nullObj = null;

            mockedDB.Setup(db => db.ReadExchangeAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency))
                .ReturnsAsync(nullObj);
            mockedDB.Setup(db => db.CreateExchangeAsync(expectedResult))
                .ReturnsAsync(expectedResult);

            mockExternalAPI.Setup(api => api.GetExchangeAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency))
                .ReturnsAsync(expectedResult);

            var testedCode = new Exchange(mockedDB.Object, mockExternalAPI.Object, mockedLogger.Object);

            // act
            var result = await testedCode.GetAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency);

            // assert 
            Assert.NotNull(result?.Result);
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultTestValue = Assert.IsType<ExchangeRates.Models.Exchange>(actionResult.Value);
            Assert.Equal(expectedResult, resultTestValue);
        }

        [Fact]
        public async Task Get_NonExistingAsync()
        {
            // arrange
            var mockedDB = new Mock<IDatabase>();
            var mockExternalAPI = new Mock<IExternalAPI>();
            var mockedLogger = new Mock<ILogger<Exchange>>();

            var expectedResult = new ExchangeRates.Models.Exchange
            {
                SourceCurrency = "eur",
                DestinationCurrency = "usd",
                BidValue = 1.002M,
                AskValue = 0.9988M
            };
            ExchangeRates.Models.Exchange nullObj = null;

            mockedDB.Setup(db => db.ReadExchangeAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency))
                .ReturnsAsync(nullObj);
            mockedDB.Setup(db => db.CreateExchangeAsync(expectedResult))
                .ReturnsAsync(expectedResult);

            mockExternalAPI.Setup(api => api.GetExchangeAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency))
                .ReturnsAsync(nullObj);

            var testedCode = new Exchange(mockedDB.Object, mockExternalAPI.Object, mockedLogger.Object);

            // act
            var result = await testedCode.GetAsync(expectedResult.SourceCurrency, expectedResult.DestinationCurrency);

            // assert 
            Assert.NotNull(result?.Result);
            var actionResult = Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}