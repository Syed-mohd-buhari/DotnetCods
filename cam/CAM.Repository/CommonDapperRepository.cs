using Dapper;
using OracleModels.DBContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository
{
    public   class CommonDapperRepository
    {
      
        private readonly DapperContext _context;

        public CommonDapperRepository(
            DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string mode,
            string sql,
            object param = null)
        {
            using var connection =
                _context.CreateConnection(mode);

            return await connection.QueryAsync<T>(
                sql,
                param);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(
          string mode,
          string sql,
          object param = null)
        {
            using var connection = _context.CreateConnection(mode);

            return await connection.QueryFirstOrDefaultAsync<T>(
                sql,
                param);
        }
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(
     string mode,
     string sql,
     object param = null)
        {
            using var connection = _context.CreateConnection(mode);

            return await connection.QueryMultipleAsync(sql, param);
        }
    }
}