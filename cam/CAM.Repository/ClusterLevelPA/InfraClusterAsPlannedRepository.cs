using CAM.Contracts.RepositoryContracts.ClusterLevelPA;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CluserLevelPA
{
    public class InfraClusterAsPlannedRepository : RepositoryBase<Infraclusterasplanned>, IInfraClusterAsPlannedRepository
    {
        ModelContext _context;
        public InfraClusterAsPlannedRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
