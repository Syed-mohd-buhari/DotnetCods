using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfClusterRepository : RepositoryBaseNew<Cnfcluster>, ICnfClusterRepository
    {
        ModelContextNew _context;
        public CnfClusterRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
