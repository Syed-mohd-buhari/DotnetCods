using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class DesignAspectNetworkFunctionRepository : RepositoryBaseNew<Designaspectsnetworkfunctions>, IDesignAspectNetworkFunctionRepository
    {
        public DesignAspectNetworkFunctionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
