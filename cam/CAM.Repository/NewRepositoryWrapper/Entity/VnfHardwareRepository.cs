using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class VnfHardwareRepository : RepositoryBaseNew<Vnfhardware>, IVnfHardwareRepository
    {
        ModelContextNew _context;
        public VnfHardwareRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
