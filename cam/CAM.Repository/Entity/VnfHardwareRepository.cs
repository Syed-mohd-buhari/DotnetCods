using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class VnfHardwareRepository : RepositoryBase<Vnfhardware>, IVnfHardwareRepository
    {
        public VnfHardwareRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
