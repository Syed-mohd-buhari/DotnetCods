using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class ServiceMasterRepository : RepositoryBaseNew<Servicemaster>, IServiceMasterRepository
    {
       public ServiceMasterRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
