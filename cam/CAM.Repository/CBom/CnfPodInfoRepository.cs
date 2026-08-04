using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfPodInfoRepository : RepositoryBase<Cnfpodinfo>, ICnfPodInfoRepository
    {
        ModelContext _context;
        public CnfPodInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}

