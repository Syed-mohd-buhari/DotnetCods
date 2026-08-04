using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfCapacityRepository : RepositoryBaseNew<Cnfcapacity>, ICnfCapacityRepository
    {
        ModelContextNew _context;
        public CnfCapacityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
