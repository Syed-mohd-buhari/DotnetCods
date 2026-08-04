using CAM.Contracts.RepositoryContracts.CBOM;
using CAM.Entities.Models.CBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    internal class PodTypeInfoRepository : RepositoryBaseNew<Podtypeinfo>, IPodTypeInfoRepository
    {
        ModelContextNew _context;
        public PodTypeInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
