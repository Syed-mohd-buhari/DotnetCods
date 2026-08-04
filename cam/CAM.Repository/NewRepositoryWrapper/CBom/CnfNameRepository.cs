using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfNameRepository : RepositoryBaseNew<Cnfname>, ICnfNameRepository
    {
        ModelContextNew _context;
        public CnfNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
