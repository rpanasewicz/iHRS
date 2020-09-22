using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace iHRS.Infrastructure.Queries
{
    internal abstract class DapperQueryBase
    {
        private readonly IConfiguration _configuration;

        protected DapperQueryBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Get<T>(Func<IDbConnection, T> query)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("Default"));
            return query.Invoke(db);
        }

        protected void Execute(Action<IDbConnection> query)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("Default"));
            query.Invoke(db);
        }
    }
}
