using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfPriorityRepository : RepositoryBaseNew<Cnfpriority>, ICnfPriorityRepository
    {
        ModelContextNew _context;
        public CnfPriorityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}

