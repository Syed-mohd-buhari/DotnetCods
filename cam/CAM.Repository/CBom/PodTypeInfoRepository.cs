using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class PodTypeInfoRepository : RepositoryBase<Podtypeinfo>, IPodTypeInfoRepository
    {
        ModelContext _context;
        public PodTypeInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
