namespace wdt_banking_cli.DAO;
using wdt_banking_cli.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class MCBAAccountDAO : IAccountDAO
{
    public async Task CreateAccountAsync(Account account)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = @"INSERT INTO Account (AccountNumber, AccountType, CustomerID, Balance)
        VALUES (@1, @2, @3, @4)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@1", account.AccountNumber);
        command.Parameters.AddWithValue("@2", account.AccountType.ToString());
        command.Parameters.AddWithValue("@3", account.CustomerID);
        command.Parameters.AddWithValue("@4", account.Balance);
        await command.ExecuteNonQueryAsync();
    }

    public Account? GetAccountByAccountNumber(int accountNumber)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM Account WHERE AccountNumber = @1";
        command.Parameters.AddWithValue("@1", accountNumber);
        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);
        var rows = table.Select();
        if (rows.Length < 1) return null;
        var firstRow = rows[0];

        var accountType = firstRow.Field<string>("AccountType");
        Enum.TryParse(accountType, out AccountType parsedAccountType);

        return new Account(
                    firstRow.Field<int>("AccountNumber"),
                    parsedAccountType,
                    firstRow.Field<int>("CustomerID"),
                    firstRow.Field<decimal>("Balance")
                );
    }

    public List<Account> GetAccountsByCustomerID(int customerID)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM Account WHERE CustomerID = @1";
        command.Parameters.AddWithValue("@1", customerID);
        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);
        var accounts = new List<Account>();
        foreach (DataRow row in table.Select())
        {
            var accountType = row.Field<string>("AccountType");
            if (accountType == null) continue;
            var parseRes = Enum.TryParse(accountType, out AccountType parsedAccountType);
            if (!parseRes) continue;

            accounts.Add(
                new Account(
                    row.Field<int>("AccountNumber"),
                    parsedAccountType,
                    row.Field<int>("CustomerID"),
                    row.Field<decimal>("Balance")
                ));
        }

        return accounts;
    }

    public void UpdateAccount(Account account)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = @"UPDATE Account SET AccountType = @1, CustomerID = @2, Balance = @3 WHERE AccountNumber = @4";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@1", account.AccountType.ToString());
        command.Parameters.AddWithValue("@2", account.CustomerID);
        command.Parameters.AddWithValue("@3", account.Balance);
        command.Parameters.AddWithValue("@4", account.AccountNumber);
        command.ExecuteNonQuery();
    }
}