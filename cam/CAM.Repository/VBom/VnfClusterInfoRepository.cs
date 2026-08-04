using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VnfClusterInfoRepository : RepositoryBase<Vnfclusterinfo>, IVnfClusterInfoRepository
    {
        ModelContext _context;
        public VnfClusterInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
 