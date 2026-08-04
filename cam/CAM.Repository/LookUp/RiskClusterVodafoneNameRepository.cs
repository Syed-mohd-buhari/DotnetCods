using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public partial class RiskClusterVodafoneNameRepository : RepositoryBase<Riskclustervodafonenames>, IRiskClusterVodafoneNamesRepository
    {
       public RiskClusterVodafoneNameRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
