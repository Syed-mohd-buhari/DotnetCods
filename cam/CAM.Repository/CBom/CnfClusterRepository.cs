using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class CnfClusterRepository : RepositoryBase<Cnfcluster>, ICnfClusterRepository
    {
        ModelContext _context;
        public CnfClusterRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
