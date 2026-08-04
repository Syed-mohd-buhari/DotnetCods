using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class ClusterNameRepository : RepositoryBase<Clustername>, IClusterNameRepository
    {
        ModelContext _context;
        public ClusterNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
