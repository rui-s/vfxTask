create or alter procedure dbo.DeleteExchange
(
      	@sourceCurrency    	nvarchar(10),
      	@destinationCurrency nvarchar(10)
)
as
begin
	-- validate if record exists
	if not exists (select 1 from dbo.CurrencyExchangeRate where sourceCurrency = @sourceCurrency AND destinationCurrency = @destinationCurrency)
	begin
		THROW 51002, 'The record does not exists.', 1;
	end

	delete dbo.CurrencyExchangeRate 
	where sourceCurrency = @sourceCurrency
		and destinationCurrency = @destinationCurrency;
end