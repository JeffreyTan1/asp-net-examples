namespace wdt_banking_cli.DAO;
using wdt_banking_cli.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class MCBACustomerDAO : ICustomerDAO
{
    public int GetCustomerCount()
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = "SELECT COUNT(*) FROM Customer";
        using SqlCommand command = new SqlCommand(query, connection);
        return (int)command.ExecuteScalar();
    }

    public async Task CreateCustomerAsync(Customer customer)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        connection.Open();
        var query = @"INSERT INTO Customer (CustomerID, Name, Address, City, PostCode)
        VALUES (@1, @2, @3, @4, @5)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@1", customer.CustomerID);
        command.Parameters.AddWithValue("@2", customer.Name);
        command.Parameters.AddWithValue("@3", customer.Address ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@4", customer.City ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@5", customer.PostCode ?? (object)DBNull.Value);
        await command.ExecuteNonQueryAsync();
    }
    
    public Customer? GetCustomerByCustomerID(int customerID)
    {
        using var connection = new SqlConnection(Secrets.DBConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM Customer WHERE CustomerID = @1";
        command.Parameters.AddWithValue("@1", customerID);

        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);
        var rows = table.Select();

        if (rows.Length < 1) return null;
        var firstRow = rows[0];
        return new Customer(
                    firstRow.Field<int>("CustomerID"),
                    firstRow.Field<string>("Name") ?? "",
                    firstRow.Field<string?>("Address"),
                    firstRow.Field<string?>("City"),
                    firstRow.Field<string?>("Postcode")
                );
    }
}