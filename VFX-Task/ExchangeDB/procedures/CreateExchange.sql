create or alter procedure dbo.CreateExchange
(
      	@sourceCurrency    	nvarchar(10),
      	@destinationCurrency nvarchar(10),
      	@bidValue			decimal(12,4),
     	@askValue			decimal(12,4)
)
as
begin
	-- validate id record exists
	if exists (select 1 from dbo.CurrencyExchangeRate where sourceCurrency = @sourceCurrency AND destinationCurrency = @destinationCurrency)
	begin
		THROW 51000, 'The record already exists.', 1;
	end

	INSERT INTO dbo.CurrencyExchangeRate (sourceCurrency, destinationCurrency, bidValue, askValue)
	VALUES (@sourceCurrency, @destinationCurrency, @bidValue, @askValue);
end