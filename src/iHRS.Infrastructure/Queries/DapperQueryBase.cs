using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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

        protected IEnumerable<T> Get<T>(string query, object param = null)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("Default"));
            return db.Query<T>(query, param);
        }
    }
}
