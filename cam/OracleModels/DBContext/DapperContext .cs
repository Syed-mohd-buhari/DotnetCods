using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
 

namespace OracleModels.DBContext
{
    public class DapperContext  
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            return new OracleConnection(
                _configuration.GetConnectionString("oracle"));
        }
        public IDbConnection CreateConnection(
            string mode)
        {
            string connectionString =
                mode == "training"
                ? _configuration.GetConnectionString(
                    "oraclenew")
                : _configuration.GetConnectionString(
                    "oracle");

            return new OracleConnection(
                connectionString);
        }
    }
}