create or alter procedure dbo.UpdateExchange
(
      	@sourceCurrency    	nvarchar(10),
      	@destinationCurrency nvarchar(10),
      	@bidValue			decimal(12,4),
     	@askValue			decimal(12,4)
)
as
begin
	-- validate if record exists
	if not exists (select 1 from dbo.CurrencyExchangeRate where sourceCurrency = @sourceCurrency AND destinationCurrency = @destinationCurrency)
	begin
		THROW 51001, 'The record does not exists.', 1;
	end

	update dbo.CurrencyExchangeRate 
	set bidValue = @bidValue, askValue = @askValue
	where sourceCurrency = @sourceCurrency
		and destinationCurrency = @destinationCurrency;
end
