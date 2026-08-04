using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class DaAssetMigrationRepository : RepositoryBase<Daassetmigration>, IDaAssetMigrationRepository
    {
        public DaAssetMigrationRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
