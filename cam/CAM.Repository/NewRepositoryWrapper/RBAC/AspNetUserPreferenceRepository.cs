using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class AspNetUserPreferenceRepository : RepositoryBaseNew<Aspnetuserpreferences>, IAspNetUserPreferenceRepository
    {
        ModelContextNew _context;
        public AspNetUserPreferenceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
