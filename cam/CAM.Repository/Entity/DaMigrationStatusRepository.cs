using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class DaMigrationStatusRepository : RepositoryBase<Damigrationstatus>, IDaMigrationStatusRepository
    {
        public DaMigrationStatusRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
