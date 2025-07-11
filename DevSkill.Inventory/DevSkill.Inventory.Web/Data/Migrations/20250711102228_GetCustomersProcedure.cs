using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetCustomersProcedure : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                GO
                CREATE OR ALTER PROCEDURE [dbo].[GetCustomers] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Name nvarchar(max) = '%',
                	@CustomerId nvarchar(20) = '%',
                	@Mobile nvarchar(max) = '%',	
                	@Address nvarchar(max) = '%',
                	@Email nvarchar(max) = '%',
                	@BalanceFrom int = NULL,
                	@BalanceTo int = NULL,	
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @countsql nvarchar(2000);	 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from Customers;

                	-- Collecting Total Display
                	SET @countsql = 'SELECT @TotalDisplay = COUNT(*) FROM Customers C WHERE 1 = 1 ';

                	SET @countsql = @countsql + ' AND C.Name LIKE ''%'' + @xName + ''%''' 	
                	SET @CountSql = @CountSql + ' AND C.CustomerId LIKE ''%'' + @xCustomerId + ''%'''
                	SET @CountSql = @CountSql + ' AND C.Mobile LIKE ''%'' + @xMobile + ''%'''
                	SET @CountSql = @CountSql + ' AND C.Address LIKE ''%'' + @xAddress + ''%'''
                	SET @CountSql = @CountSql + ' AND C.Email LIKE ''%'' + @xEmail + ''%'''

                	IF @BalanceFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND C.Balance >= @xBalanceFrom'

                	IF @BalanceTo IS NOT NULL
                	SET @countsql = @countsql + ' AND C.Balance <= @xBalanceTo' 	 

                	SELECT @countparamlist = '
                		@xName nvarchar(max),
                		@xCustomerId nvarchar(20),
                		@xMobile nvarchar(max),
                		@xAddress nvarchar(max),
                		@xEmail nvarchar(max),
                		@xBalanceFrom int,
                		@xBalanceTo int,		
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Name,
                		@CustomerId,
                		@Mobile,
                		@Address,
                		@Email,
                		@BalanceFrom,
                		@BalanceTo,				
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	Declare @sql nvarchar(2000);
                	Declare @paramList nvarchar(MAX);

                	SET @sql = 'SELECT 
                					C.Id,
                					C.Name,
                					C.CustomerId,
                					C.Mobile,
                					C.Address,
                					C.Status,
                					C.Balance,
                					C.ImageUrl,
                					C.Email
                					FROM Customers C WHERE 1 = 1 '					

                	SET @sql = @sql + ' AND C.Name LIKE ''%'' + @xName + ''%''' 	
                	SET @Sql = @Sql + ' AND C.CustomerId LIKE ''%'' + @xCustomerId + ''%'''
                	SET @Sql = @Sql + ' AND C.Mobile LIKE ''%'' + @xMobile + ''%'''
                	SET @Sql = @Sql + ' AND C.Address LIKE ''%'' + @xAddress + ''%'''
                	SET @Sql = @Sql + ' AND C.Email LIKE ''%'' + @xEmail + ''%'''

                	IF @BalanceFrom IS NOT NULL
                	SET @sql = @sql + ' AND C.Balance >= @xBalanceFrom'

                	IF @BalanceTo IS NOT NULL
                	SET @sql = @sql + ' AND C.Balance <= @xBalanceTo' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '
                		@xName nvarchar(max),
                		@xCustomerId nvarchar(20),
                		@xMobile nvarchar(max),
                		@xAddress nvarchar(max),
                		@xEmail nvarchar(max),
                		@xBalanceFrom int,
                		@xBalanceTo int,
                		@PageIndex int,
                		@PageSize int';

                	exec sp_executesql @sql , @paramlist ,
                		@Name,
                		@CustomerId,
                		@Mobile,
                		@Address,
                		@Email,
                		@BalanceFrom,
                		@BalanceTo,
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
            var sql = "DROP PROCEDURE [dbo].[GetCustomers]";
            migrationBuilder.Sql(sql);
        }
    }
}
