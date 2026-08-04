using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class ServicePlanDcfMappingRepository : RepositoryBaseNew<Serviceplandcfmappings>, IServicePlanDcfMappingRepository
    {
        public ServicePlanDcfMappingRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
