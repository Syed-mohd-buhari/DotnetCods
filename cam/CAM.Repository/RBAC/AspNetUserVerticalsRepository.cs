using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class AspNetUserVerticalsRepository : RepositoryBase<Aspnetuserverticals>, IAspNetUserVerticalsRepository
    {
        ModelContext _context;
        public AspNetUserVerticalsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
