using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TestCustomerWinForms.Net8
{
    public class CustomerRepository
    {
        private readonly string _connString;

        public CustomerRepository()
        {
            _connString = AppConfig.ConnString;
        }

        public List<Customer> GetAll()
        {
            //// Just checking github

            var list = new List<Customer>();
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("SELECT CustomerID, CustomerName FROM dbo.tblTestCustomer ORDER BY CustomerID", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Customer
                {
                    CustomerID = reader.GetInt32(0),
                    CustomerName = reader.IsDBNull(1) ? "" : reader.GetString(1)
                });
            }
            return list;
        }

        public int Add(Customer c)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.tblTestCustomer (CustomerName) VALUES (@name);
                                             SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = (object?)c.CustomerName ?? DBNull.Value;
            conn.Open();
            var newId = (int)cmd.ExecuteScalar()!;
            return newId;
        }

        public void Update(Customer c)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("UPDATE dbo.tblTestCustomer SET CustomerName=@name WHERE CustomerID=@id", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = (object?)c.CustomerName ?? DBNull.Value;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = c.CustomerID;
            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) throw new InvalidOperationException("No rows updated. Check CustomerID.");
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("DELETE FROM dbo.tblTestCustomer WHERE CustomerID=@id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}