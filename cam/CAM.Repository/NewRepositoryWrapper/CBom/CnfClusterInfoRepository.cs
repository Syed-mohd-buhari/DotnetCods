using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfClusterInfoRepository : RepositoryBaseNew<Cnfclusterinfo>, ICnfClusterInfoRepository
    {
        ModelContextNew _context;
        public CnfClusterInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
} 