using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class RiskClusterRepository : RepositoryBaseNew<Riskclusters>, IRiskClusterRepository
    {
       public RiskClusterRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
