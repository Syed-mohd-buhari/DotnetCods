using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class RiskClusterVodafoneNameRepository : RepositoryBaseNew<Riskclustervodafonenames>, IRiskClusterVodafoneNamesRepository
    {
       public RiskClusterVodafoneNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
