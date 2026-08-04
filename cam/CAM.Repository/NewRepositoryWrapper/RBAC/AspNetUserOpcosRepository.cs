using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class AspNetUserOpcosRepository : RepositoryBaseNew<Aspnetuseropcos>, IAspNetUserOpcosRepository
    {
        ModelContextNew _context;
        public AspNetUserOpcosRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
