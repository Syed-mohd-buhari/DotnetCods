using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfCapacityRepository : RepositoryBase<Cnfcapacity>, ICnfCapacityRepository
    {
        ModelContext _context;
        public CnfCapacityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
