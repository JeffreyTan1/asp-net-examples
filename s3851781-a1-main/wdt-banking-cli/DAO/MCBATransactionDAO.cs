namespace wdt_banking_cli.DAO;
using wdt_banking_cli.Models;

using Microsoft.Data.SqlClient;
using System.Data;

public class MCBATransactionDAO : ITransactionDAO
{
    public async Task CreateTransactionAsync(TransactionCreate transaction)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = @"INSERT INTO [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)
        VALUES (@1, @2, @3, @4, @5, @6)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@1", transaction.TransactionType.ToString());
        command.Parameters.AddWithValue("@2", transaction.AccountNumber);
        command.Parameters.AddWithValue("@3", transaction.DesinationAccountNumber ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@4", transaction.Amount);
        command.Parameters.AddWithValue("@5", transaction.Comment ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@6", transaction.TransactionTimeUtc);
        await command.ExecuteNonQueryAsync();
    }

    public List<Transaction> GetTransactionsByAccountNumber(int accountNumber)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM [Transaction] WHERE AccountNumber = @1 ORDER BY TransactionTimeUtc DESC";
        command.Parameters.AddWithValue("@1", accountNumber);

        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);

        var transactions = new List<Transaction>();

        foreach (DataRow row in table.Select())
        {
            var transactionType = row.Field<string>("TransactionType");
            if (transactionType == null) continue;
            var parseRes = Enum.TryParse(transactionType, out TransactionType parsedTransactionType);
            if (!parseRes) continue;

            transactions.Add(
                new Transaction(
                    row.Field<int>("TransactionID"),
                    parsedTransactionType,
                    row.Field<int>("AccountNumber"),
                    row.Field<int?>("DestinationAccountNumber"),
                    row.Field<decimal>("Amount"),
                    row.Field<string?>("Comment"),
                    row.Field<DateTime>("TransactionTimeUtc")
                ));
        }
        return transactions;

    }

}