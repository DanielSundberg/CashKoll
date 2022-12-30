using Microsoft.Data.Sqlite;
using Dapper;

namespace CashKollen.Data;


public class DbService
{
    private readonly string connectionString = "";

    public DbService(IConfiguration configuration)
    {
        connectionString = configuration["ConnectionString"] ?? "";
    }

    public Task<AccountSummary[]> GetAccountSummay()
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<AccountSummary>(@"
                SELECT 
                    c.name as 'Name',
                    COUNT(*) as 'Count',
                    SUM(t.amount) as 'Amount'
                FROM trans t
                    JOIN category c ON t.category=c.id
                WHERE t.deleted=0
                GROUP BY category
                ").ToArray()
            );
        }
    }


    public Task<AccountSummary[]> GetAccountSummayThisYear(int accountId, IEnumerable<string> selectedYears, IEnumerable<string> selectedMonths)
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<AccountSummary>(@"
                    SELECT 
                        strftime('%Y',t.timestamp) AS 'Year',
                        strftime('%m',t.timestamp) AS 'Month',
                        c.name as 'Name',
                        SUM(t.Amount)/1000 AS 'Amount'
                    FROM trans t
                        LEFT JOIN category c ON t.category=c.id
                    WHERE YEAR IN @years AND MONTH IN @months AND account=@accountId AND t.deleted=0
                        GROUP BY month,year,category
                        ORDER BY MONTH,YEAR
                    ", 
                    new { years = selectedYears, months = selectedMonths, accountId }
                )
                .ToArray()
            );
        }
    }

    public Task<AccountSummary[]> GetAccountSummayAllYears(int accountId)
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<AccountSummary>(@"
                    SELECT 
                        strftime('%Y',t.timestamp) AS 'Year',
                        c.name as 'Name',
                        SUM(t.Amount)/1000 AS 'Amount'
                    FROM trans t
                        LEFT JOIN category c ON t.category=c.id
                    WHERE Account=@accountId AND t.deleted=0
                    GROUP BY YEAR,category
                    ORDER BY YEAR
                    ", 
                    new { accountId }
                )
                .ToArray()
            );
        }
    }
    

    public Task<Category[]> GetCategories() {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<Category>(@"
                SELECT 
                    c.id as 'Id',
                    c.name as 'Name'
                FROM Category c")
                .ToArray()
            );
        }
    }

    public Task<CategoryMap[]> GetCategoryMap() {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<CategoryMap>(@"
                SELECT 
                    id as 'Id',
                    name as 'Name',
                    expression as 'Expression',
                    category as 'CategoryId'
                FROM category_map")
                .ToArray()
            );
        }
    }

    public Task<int> SaveCategoryMap(string expression, int categoryId) {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return dbConnection.ExecuteAsync(
                "INSERT INTO category_map (category,expression,TIMESTAMP) VALUES (@categoryId, @expression, DATETIME('now'));", 
                new { categoryId, expression }
            );
        }
    }

    public Task<AccountLine[]> SearchTrans(int accountId, DateTime from, DateTime to, int category = 0, string searchText = "")
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            var categoryWhereClause = category > 0 ? " AND Category=@category" : "";
            var searchTextWithWildcards = $"%{searchText}%";
            var searchTextWhereClause = !string.IsNullOrWhiteSpace(searchText) ? " AND t.Description like @searchTextWithWildcards" : "";
            return Task.FromResult(dbConnection.Query<AccountLine>(@$"
                    SELECT 
                        t.id AS 'Id',
                        t.timestamp AS 'Timestamp',
                        t.account AS 'Account',
                        t.category AS 'Category',
                        c.name AS 'CategoryName',
                        t.currency AS 'Currency',
                        t.Amount AS 'Amount',
                        t.balance AS 'Balance',
                        t.Description AS 'Description'
                    FROM trans t
                        JOIN category c ON t.category=c.id
                    WHERE 
                        t.timestamp>=@from AND 
                        t.timestamp<=@to AND 
                        t.account=@accountId AND 
                        t.deleted=0
                        {categoryWhereClause}
                        {searchTextWhereClause}
                    ORDER BY t.timestamp DESC
                ", new { accountId, from, to, category, searchTextWithWildcards })
                .ToArray()
            );
        }
    }

    public Task<int> SaveTrans(AccountLine line)
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return dbConnection.ExecuteAsync(
                "INSERT INTO trans(timestamp, description, account, currency, amount, balance, category) VALUES (@date, @descr, @account, @currency, @amount, @balance, @category);", 
                new {
                    date = line.Timestamp, 
                    descr = line.Description,
                    account = line.Account,
                    currency = line.Currency, 
                    amount = line.Amount, 
                    balance = line.Balance,
                    category = line.Category
                }
            );
        }
    }

    public Task<int> UpdateTrans(AccountLine line)
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return dbConnection.ExecuteAsync(
                "UPDATE trans SET timestamp=@timestamp, description=@descr, amount=@amount, balance=@balance, category=@category WHERE Id=@id", 
                new {
                    timestamp = line.Timestamp, 
                    descr = line.Description,
                    amount = line.Amount, 
                    balance = line.Balance,
                    category = line.Category,
                    id = line.Id
                }
            );
        }
    }

    public Task<int> DeleteTrans(AccountLine line)
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return dbConnection.ExecuteAsync(
                "UPDATE trans SET deleted=1 WHERE Id=@id", 
                new {
                    id = line.Id
                }
            );
        }
    }

    public Task<Account[]> GetAccouts()
    {
        using (var dbConnection = new SqliteConnection(connectionString))
        {
            return Task.FromResult(dbConnection.Query<Account>(@"
                SELECT 
                        id as 'Id',
                        name as 'Name'
                    FROM account"
                )
                .ToArray()
            );
        }
    }
}
