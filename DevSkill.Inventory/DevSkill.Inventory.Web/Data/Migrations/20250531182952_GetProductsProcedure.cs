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
                CREATE OR ALTER PROCEDURE [dbo].[GetProducts] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Name nvarchar(max) = '%',
                	@Description nvarchar(max) = '%',
                	@PriceFrom int = NULL,
                	@PriceTo int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @sql nvarchar(2000);
                	Declare @countsql nvarchar(2000);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from Products;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from Products p where 1 = 1 ';

                	SET @countsql = @countsql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	SET @countsql = @countsql + ' AND p.Description LIKE ''%'' + @xDescription + ''%''' 

                	IF @PriceFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND p.Price >= @xPriceFrom'

                	IF @PriceTo IS NOT NULL
                	SET @countsql = @countsql + ' AND p.Price <= @xPriceTo' 

                	SELECT @countparamlist = '@xName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xPriceFrom int,
                		@xPriceTo int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Name,
                		@Description,
                		@PriceFrom,
                		@PriceTo,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = 'select * from Products p where 1 = 1 ';

                	SET @sql = @sql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	SET @sql = @sql + ' AND p.Description LIKE ''%'' + @xDescription + ''%''' 

                	IF @PriceFrom IS NOT NULL
                	SET @sql = @sql + ' AND p.Price >= @xPriceFrom'

                	IF @PriceTo IS NOT NULL
                	SET @sql = @sql + ' AND p.Price <= @xPriceTo' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xPriceFrom int,
                		@xPriceTo int,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Name,
                		@Description,
                		@PriceFrom,
                		@PriceTo,
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
