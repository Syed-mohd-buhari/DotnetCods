using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfClusterInfoRepository : RepositoryBase<Cnfclusterinfo>, ICnfClusterInfoRepository
    {
        ModelContext _context;
        public CnfClusterInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
