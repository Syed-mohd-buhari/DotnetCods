using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public partial class RiskClusterRepository : RepositoryBase<Riskclusters>, IRiskClusterRepository
    {
       public RiskClusterRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
