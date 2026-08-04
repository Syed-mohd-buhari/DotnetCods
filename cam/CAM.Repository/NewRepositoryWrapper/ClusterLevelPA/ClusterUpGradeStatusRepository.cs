using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;
namespace CAM.Repository.NewRepositoryWrapper.CluserLevelPA
{
    public class ClusterUpGradeStatusRepository : RepositoryBaseNew<Clusterupgradestatus>, IClusterUpGradeStatusRepository
    {
        ModelContextNew _context;
        public ClusterUpGradeStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
