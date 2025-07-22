using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetSalesProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """                       
                GO
                CREATE OR ALTER PROCEDURE [dbo].[GetSales] 
                	@PageIndex int,
                	@PageSize int, 
                	@OrderBy nvarchar(50),
                	@InvoiceNo nvarchar(50) = '%',
                	@CustomerName nvarchar(max) = '%',
                	@Status nvarchar(50) = '%',
                	@SaleDateFrom datetime = NULL,
                	@SaleDateTo datetime = NULL,
                	@TotalPriceFrom int = NULL,
                	@TotalPriceTo int = NULL,
                	@PaidAmount int = NULL,
                	@DueAmount int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @countsql nvarchar(4000);	 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from Sales;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = COUNT(*) FROM( SELECT DISTINCT S.Id
                						FROM Sales S
                						INNER JOIN Customers C ON S.CustomerId = C.Id
                						WHERE 1 = 1 ';

                	SET @countsql = @countsql + ' AND S.InvoiceNo LIKE ''%'' + @xInvoiceNo + ''%''' 	
                	SET @CountSql = @CountSql + ' AND C.Name LIKE ''%'' + @xCustomerName + ''%'''

                	IF @Status != '%'
                		SET @CountSql = @CountSql + ' AND S.Status LIKE ''%'' + @xStatus + ''%'''

                	IF @SaleDateFrom IS NOT NULL
                		SET @countsql = @countsql + ' AND S.SaleDate >= @xSaleDateFrom'

                	IF @SaleDateTo IS NOT NULL
                		SET @countsql = @countsql + ' AND S.SaleDate <= @xSaleDateTo' 

                	IF @TotalPriceFrom IS NOT NULL
                		SET @countsql = @countsql + ' AND S.TotalAmount >= @xTotalPriceFrom'

                	IF @TotalPriceTo IS NOT NULL
                		SET @countsql = @countsql + ' AND S.TotalAmount <= @xTotalPriceTo' 

                	IF @PaidAmount IS NOT NULL
                		SET @countsql = @countsql + ' AND S.PaidAmount = @xPaidAmount'

                	IF @DueAmount IS NOT NULL
                		SET @countsql = @countsql + ' AND S.DueAmount = @xDueAmount'

                	SET @countSql = @countSql + ') AS FilteredItems ';

                	SELECT @countparamlist = '
                		@xInvoiceNo nvarchar(50),
                		@xCustomerName nvarchar(max),
                		@xStatus nvarchar(50),
                		@xSaleDateFrom datetime,
                		@xSaleDateTo datetime,
                		@xTotalPriceFrom int,
                		@xTotalPriceTo int,
                		@xPaidAmount int,
                		@xDueAmount int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@InvoiceNo,
                		@CustomerName,
                		@Status,
                		@SaleDateFrom,
                		@SaleDateTo,
                		@TotalPriceFrom,
                		@TotalPriceTo,
                		@PaidAmount,
                		@DueAmount,				
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	Declare @sql nvarchar(4000);
                	Declare @paramList nvarchar(MAX);

                	SET @sql = 'select
                					S.Id,
                					S.InvoiceNo,
                					S.SaleDate,
                					C.Name as CustomerName,
                					S.TotalAmount,
                					S.PaidAmount,
                					S.DueAmount,
                					S.Status,
                					S.NetAmount,
                					S.Discount,
                					S.VatAmount,
                					S.Notes,
                					S.Terms

                					FROM Sales S
                					INNER JOIN Customers C ON S.CustomerId = C.Id
                					WHERE 1 = 1 ';

                	SET @sql = @sql + ' AND S.InvoiceNo LIKE ''%'' + @xInvoiceNo + ''%''' 
                	SET @sql = @sql + ' AND C.Name LIKE ''%'' + @xCustomerName + ''%'''

                	IF @Status != '%'
                		SET @sql = @sql + ' AND S.Status LIKE ''%'' + @xStatus + ''%''' 	

                	IF @SaleDateFrom IS NOT NULL
                		SET @sql = @sql + ' AND S.SaleDate >= @xSaleDateFrom'

                	IF @SaleDateTo IS NOT NULL
                		SET @sql = @sql + ' AND S.SaleDate <= @xSaleDateTo'

                	IF @TotalPriceFrom IS NOT NULL
                		SET @sql = @sql + ' AND S.TotalAmount >= @xTotalPriceFrom'

                	IF @TotalPriceTo IS NOT NULL
                		SET @sql = @sql + ' AND S.TotalAmount <= @xTotalPriceTo'

                	IF @PaidAmount IS NOT NULL
                		SET @sql = @sql + ' AND S.PaidAmount = @xPaidAmount'

                	IF @DueAmount IS NOT NULL
                		SET @sql = @sql + ' AND S.DueAmount = @xDueAmount'

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '
                		@xInvoiceNo nvarchar(50),
                		@xCustomerName nvarchar(max),
                		@xStatus nvarchar(50),
                		@xSaleDateFrom datetime,
                		@xSaleDateTo datetime,
                		@xTotalPriceFrom int,
                		@xTotalPriceTo int,
                		@xPaidAmount int,
                		@xDueAmount int,
                		@PageIndex int,
                		@PageSize int';

                	exec sp_executesql @sql , @paramlist ,
                		@InvoiceNo,
                		@CustomerName,
                		@Status,
                		@SaleDateFrom,
                		@SaleDateTo,
                		@TotalPriceFrom,
                		@TotalPriceTo,
                		@PaidAmount,
                		@DueAmount,
                		@PageIndex,
                		@PageSize;

                	print @sql;
                	print @countsql;

                END
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetSales]";
            migrationBuilder.Sql(sql);
        }
    }
}
