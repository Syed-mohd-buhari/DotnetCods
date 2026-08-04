using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class AspNetUserOpcosRepository : RepositoryBase<Aspnetuseropcos>, IAspNetUserOpcosRepository
    {
        ModelContext _context;
        public AspNetUserOpcosRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
