using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class AspNetModulesRepository : RepositoryBaseNew<Aspnetmodules>, IAspNetModulesRepository
    {
        ModelContextNew _context;
        public AspNetModulesRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
