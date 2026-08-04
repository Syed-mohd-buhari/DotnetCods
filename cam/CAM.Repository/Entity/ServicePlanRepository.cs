using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class ServicePlanRepository : RepositoryBase<Serviceplan>, IServicePlanRepository
    {
        public ServicePlanRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
