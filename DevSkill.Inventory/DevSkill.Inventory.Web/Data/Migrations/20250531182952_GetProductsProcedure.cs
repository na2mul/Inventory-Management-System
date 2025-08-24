using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetProductsProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                GO
                CREATE OR ALTER   PROCEDURE [dbo].[GetProducts] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Name nvarchar(max) = '%',
                	@Barcode nvarchar(20) = '%',
                	@MeasurementUnitName nvarchar(max) = '%',	
                	@CategoryName nvarchar(max) = '%',
                	@PurchasePriceFrom int = NULL,
                	@PurchasePriceTo int = NULL,
                	@StockFrom int = NULL,
                	@StockTo int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @countsql nvarchar(2000);	 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from Products;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = COUNT(*) FROM( SELECT DISTINCT P.Id
                						FROM Products P
                						INNER JOIN MeasurementUnit MU ON P.MeasurementUnitId = MU.Id
                						LEFT JOIN Category C ON P.CategoryId = C.Id
                						WHERE 1 = 1 ';

                	SET @countsql = @countsql + ' AND P.Name LIKE ''%'' + @xName + ''%''' 	
                	SET @CountSql = @CountSql + ' AND P.Barcode LIKE ''%'' + @xBarcode + ''%'''
                	SET @CountSql = @CountSql + ' AND C.CategoryName LIKE ''%'' + @xCategoryName + ''%'''
                	SET @CountSql = @CountSql + ' AND MU.Name LIKE ''%'' + @xMeasurementUnitName + ''%'''

                	IF @PurchasePriceFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND P.PurchasePrice >= @xPurchasePriceFrom'

                	IF @PurchasePriceTo IS NOT NULL
                	SET @countsql = @countsql + ' AND P.PurchasePrice <= @xPurchasePriceTo' 

                	IF @StockFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND P.Stock >= @xStockFrom'

                	IF @StockTo IS NOT NULL
                	SET @countsql = @countsql + ' AND P.Stock <= @xStockTo' 

                	SET @countSql = @countSql + ') AS FilteredItems ';

                	SELECT @countparamlist = '
                		@xName nvarchar(max),
                		@xBarcode nvarchar(20),
                		@xCategoryName nvarchar(max),
                		@xMeasurementUnitName nvarchar(max),
                		@xPurchasePriceFrom int,
                		@xPurchasePriceTo int,
                		@xStockFrom int,
                		@xStockTo int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Name,
                		@Barcode,
                		@CategoryName,
                		@MeasurementUnitName,
                		@PurchasePriceFrom,
                		@PurchasePriceTo,
                		@StockFrom,
                		@StockTo,				
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	Declare @sql nvarchar(2000);
                	Declare @paramList nvarchar(MAX);

                	SET @sql = 'select
                					P.Id,
                					P.ImageUrl,
                					P.Barcode,
                					P.Name,
                					C.CategoryName,
                					MU.Name as MeasurementUnitName,
                					P.PurchasePrice,
                					P.Stock,
                					P.DamageStock,
                					P.LowStock,
                					P.WholesalePrice,
                					P.MRP

                					FROM Products P
                					INNER JOIN MeasurementUnit MU ON P.MeasurementUnitId = MU.Id
                					LEFT JOIN Category C ON P.CategoryId = C.Id
                					WHERE 1 = 1 ';

                	SET @sql = @sql + ' AND P.Name LIKE ''%'' + @xName + ''%''' 
                	SET @sql = @sql + ' AND C.CategoryName LIKE ''%'' + @xCategoryName + ''%'''
                	SET @sql = @sql + ' AND MU.Name LIKE ''%'' + @xMeasurementUnitName + ''%''' 	
                	SET @Sql = @Sql + ' AND P.Barcode LIKE ''%'' + @xBarcode + ''%''';

                	IF @PurchasePriceFrom IS NOT NULL
                	SET @sql = @sql + ' AND P.PurchasePrice >= @xPurchasePriceFrom'

                	IF @PurchasePriceTo IS NOT NULL
                	SET @sql = @sql + ' AND P.PurchasePrice <= @xPurchasePriceTo'

                	IF @StockFrom IS NOT NULL
                	SET @sql = @sql + ' AND P.Stock >= @xStockFrom'

                	IF @StockTo IS NOT NULL
                	SET @sql = @sql + ' AND P.Stock <= @xStockTo'

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '
                		@xName nvarchar(max),
                		@xBarcode nvarchar(20),
                		@xCategoryName nvarchar(max),
                		@xMeasurementUnitName nvarchar(max),
                		@xPurchasePriceFrom int,
                		@xPurchasePriceTo int,
                		@xStockFrom int,
                		@xStockTo int,
                		@PageIndex int,
                		@PageSize int';

                	exec sp_executesql @sql , @paramlist ,
                		@Name,
                		@Barcode,
                		@CategoryName,
                		@MeasurementUnitName,
                		@PurchasePriceFrom,
                		@PurchasePriceTo,
                		@StockFrom,
                		@StockTo,
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
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.Sql(sql);
        }
    }
}
