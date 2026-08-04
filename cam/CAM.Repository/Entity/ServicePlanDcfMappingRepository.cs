using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class ServicePlanDcfMappingRepository : RepositoryBase<Serviceplandcfmappings>, IServicePlanDcfMappingRepository
    {
        public ServicePlanDcfMappingRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
