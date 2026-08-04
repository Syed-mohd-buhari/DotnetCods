using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CluserLevelPA
{
    public class NetworkElementClusterAsPlannedRepository : RepositoryBaseNew<Networkelementclusterasplanned>, INetworkElementClusterAsPlannedRepository
    {
        ModelContextNew _context;
        public NetworkElementClusterAsPlannedRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
 