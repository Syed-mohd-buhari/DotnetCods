using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class VnfInfoRepository : RepositoryBaseNew<Vnfinfo>, IVnfInfoRepository
    {
        ModelContextNew _context;
        public VnfInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}

