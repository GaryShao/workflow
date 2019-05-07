using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SFood.Auth.Host
{
    public class DapperDbContext : IDisposable
    {
        private string _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SFoodDb");
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
