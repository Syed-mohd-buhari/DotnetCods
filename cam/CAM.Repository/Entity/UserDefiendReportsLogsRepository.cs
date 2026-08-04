using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.Entity;
namespace CAM.Repository.Entity
{
   public class UserDefinedReportsLogsRepository : RepositoryBase<Userdefinedreportslogs>, IUserDefinedReportsLogsRepository
    {
        public UserDefinedReportsLogsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
