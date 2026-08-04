using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class AspNetModulesRepository : RepositoryBase<Aspnetmodules>, IAspNetModulesRepository
    {
        ModelContext _context;
        public AspNetModulesRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
