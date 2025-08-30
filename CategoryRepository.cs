using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TestCustomerWinForms.Net8
{
    public class CategoryRepository
    {
        private readonly string _connString;

        public CategoryRepository()
        {
            _connString = AppConfig.ConnString;
        }

        public List<Category> GetAll()
        {
            var list = new List<Category>();
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("SELECT CategoryID, CategoryName, Description, Picture FROM dbo.Categories ORDER BY CategoryID", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    CategoryID = reader.GetInt32(0),
                    CategoryName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Picture = reader.IsDBNull(3) ? null : (byte[])reader[3]
                });
            }
            return list;
        }

        public int Add(Category c)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.Categories (CategoryName, Description, Picture) VALUES (@name, @desc, @pic);
                                             SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = (object?)c.CategoryName ?? DBNull.Value;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar, -1).Value = (object?)c.Description ?? DBNull.Value;
            cmd.Parameters.Add("@pic", SqlDbType.VarBinary, -1).Value = (object?)c.Picture ?? DBNull.Value;
            conn.Open();
            var newId = (int)cmd.ExecuteScalar()!;
            return newId;
        }

        public void Update(Category c)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("UPDATE dbo.Categories SET CategoryName=@name, Description=@desc, Picture=@pic WHERE CategoryID=@id", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = (object?)c.CategoryName ?? DBNull.Value;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar, -1).Value = (object?)c.Description ?? DBNull.Value;
            cmd.Parameters.Add("@pic", SqlDbType.VarBinary, -1).Value = (object?)c.Picture ?? DBNull.Value;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = c.CategoryID;
            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) throw new InvalidOperationException("No rows updated. Check CategoryID.");
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand("DELETE FROM dbo.Categories WHERE CategoryID=@id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
