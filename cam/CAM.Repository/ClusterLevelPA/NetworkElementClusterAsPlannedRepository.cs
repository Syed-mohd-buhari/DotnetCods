using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CluserLevelPA
{
    public class NetworkElementClusterAsPlannedRepository : RepositoryBase<Networkelementclusterasplanned>, INetworkElementClusterAsPlannedRepository
    {
        ModelContext _context;
        public NetworkElementClusterAsPlannedRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
