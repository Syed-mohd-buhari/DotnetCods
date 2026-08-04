using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class VnfClusterInfoRepository : RepositoryBaseNew<Vnfclusterinfo>, IVnfClusterInfoRepository
    {
        ModelContextNew _context;
        public VnfClusterInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}