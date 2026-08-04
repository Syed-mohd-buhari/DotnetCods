using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class DaMigrationStatusRepository : RepositoryBaseNew<Damigrationstatus>, IDaMigrationStatusRepository
    {
        ModelContextNew _context;
        public DaMigrationStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
