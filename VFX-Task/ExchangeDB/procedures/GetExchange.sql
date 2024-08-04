create or alter procedure dbo.GetExchange
(
	@sourceCurrency			nvarchar(10),
	@destinationCurrency	nvarchar(10)
)
as
begin

	select 
		sourceCurrency,
		destinationCurrency,
		bidValue,
		askValue
	from dbo.CurrencyExchangeRate
	where
		sourceCurrency = @sourceCurrency
		AND destinationCurrency = @destinationCurrency
		
end
