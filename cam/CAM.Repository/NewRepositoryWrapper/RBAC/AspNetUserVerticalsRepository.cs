using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class AspNetUserVerticalsRepository : RepositoryBaseNew<Aspnetuserverticals>, IAspNetUserVerticalsRepository
    {
        ModelContextNew _context;
        public AspNetUserVerticalsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
