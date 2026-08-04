using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfPriorityRepository : RepositoryBase<Cnfpriority>, ICnfPriorityRepository
    {
        ModelContext _context;
        public CnfPriorityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
