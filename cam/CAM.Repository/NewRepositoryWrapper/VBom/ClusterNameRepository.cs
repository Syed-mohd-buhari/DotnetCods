using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class ClusterNameRepository : RepositoryBaseNew<Clustername>, IClusterNameRepository
    {
        ModelContextNew _context;
        public ClusterNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
