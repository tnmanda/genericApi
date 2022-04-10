/*

DECLARE @CurrencyCode VARCHAR(10) = '{0}' --'AUD'
DECLARE @ReferenceDate DATETIME = '{1}' --'2020-04-22'
DECLARE @ReferenceType VARCHAR(10) = '{2}' --'MONTHLY'

*/

DECLARE @Date DATETIME = @ReferenceDate;

SELECT @Date = DATEADD(month, DATEDIFF(month, 0, @ReferenceDate), 0) WHERE @ReferenceType = 'MONTHLY'

SELECT TOP 1
	CurrencyCode, 
	ForexRate,
        ForexUSDtoCAD,
        ForexUSDFluctuation
FROM dbo.CurrencyForex 
WHERE CurrencyCode = @CurrencyCode AND ValidFrom <= @Date AND @Date < DATEADD(DAY, 1, ValidTo) 
ORDER BY ValidFrom