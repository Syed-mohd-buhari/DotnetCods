using System.Data;

namespace CAM.Contracts.RepositoryContracts.Base
{
    public interface IDapperContext
    { 
            IDbConnection CreateConnection();
      
    }
}