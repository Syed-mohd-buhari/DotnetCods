using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfNameRepository : RepositoryBase<Cnfname>, ICnfNameRepository
    {
        ModelContext _context;
        public CnfNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
