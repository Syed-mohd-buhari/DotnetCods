using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class CnfPodInfoRepository : RepositoryBaseNew<Cnfpodinfo>, ICnfPodInfoRepository
    {
        ModelContextNew _context;
        public CnfPodInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}

