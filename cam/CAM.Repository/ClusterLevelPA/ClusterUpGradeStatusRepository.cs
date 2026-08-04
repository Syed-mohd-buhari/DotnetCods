using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CluserLevelPA
{
    public class ClusterUpGradeStatusRepository : RepositoryBase<Clusterupgradestatus>, IClusterUpGradeStatusRepository
    {
        ModelContext _context;
        public ClusterUpGradeStatusRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
