using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfHardwareRepository : RepositoryBaseNew<Cnfhardware>, ICnfHardwareRepository
    {
        ModelContextNew _context;
        public CnfHardwareRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}

