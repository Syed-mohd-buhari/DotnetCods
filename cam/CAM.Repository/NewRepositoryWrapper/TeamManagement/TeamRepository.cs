using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class TeamRepository : RepositoryBaseNew<Teams>, ITeamRepository
    {
        ModelContextNew _context;
        public TeamRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
