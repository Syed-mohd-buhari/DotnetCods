using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class ServicePlanRepository : RepositoryBaseNew<Serviceplan>, IServicePlanRepository
    {
        public ServicePlanRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
