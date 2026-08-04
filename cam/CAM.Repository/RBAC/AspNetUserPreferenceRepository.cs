using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class AspNetUserPreferenceRepository : RepositoryBase<Aspnetuserpreferences>, IAspNetUserPreferenceRepository
    {
        ModelContext _context;
        public AspNetUserPreferenceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
