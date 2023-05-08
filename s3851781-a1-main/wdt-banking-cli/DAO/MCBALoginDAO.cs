namespace wdt_banking_cli.DAO;
using wdt_banking_cli.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class MCBALoginDAO : ILoginDAO
{
    public async Task CreateLoginAsync(Login login)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = @"INSERT INTO Login (LoginID, CustomerID, PasswordHash)
        VALUES (@1, @2, @3)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@1", login.LoginID);
        command.Parameters.AddWithValue("@2", login.CustomerID);
        command.Parameters.AddWithValue("@3", login.PasswordHash);
        await command.ExecuteNonQueryAsync();
    }

    public Login? GetLogin(string loginID)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM Login WHERE LoginID = @1";
        command.Parameters.AddWithValue("@1", loginID);

        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);

        var rows = table.Select();

        if (rows.Length < 1) return null;
        var firstRow = rows[0];

        return new Login(
            firstRow.Field<string>("LoginID") ?? "",
            firstRow.Field<int>("CustomerID"),
            firstRow.Field<string>("PasswordHash") ?? ""
        );
    }
}