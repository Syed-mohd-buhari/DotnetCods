 
using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CluserLevelPA
{
    public class InfraClusterAsPlannedRepository : RepositoryBaseNew<Infraclusterasplanned>, IInfraClusterAsPlannedRepository
    {
        ModelContextNew _context;
        public InfraClusterAsPlannedRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
