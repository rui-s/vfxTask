create table dbo.CurrencyExchangeRate
(
      sourceCurrency    nvarchar(10) not null,
      destinationCurrency nvarchar(10) not null,
      bidValue decimal(12,4) not null,
      askValue decimal(12,4) not null
);
