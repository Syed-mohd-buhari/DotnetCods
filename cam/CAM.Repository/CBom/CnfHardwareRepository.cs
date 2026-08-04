using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfHardwareRepository : RepositoryBase<Cnfhardware>, ICnfHardwareRepository
    {
        ModelContext _context;
        public CnfHardwareRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
