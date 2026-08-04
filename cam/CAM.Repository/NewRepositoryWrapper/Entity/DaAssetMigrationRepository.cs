using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class DaAssetMigrationRepository : RepositoryBaseNew<Daassetmigration>, IDaAssetMigrationRepository
    {
        ModelContextNew _context;
        public DaAssetMigrationRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
