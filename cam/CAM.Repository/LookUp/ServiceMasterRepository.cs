using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public partial class ServiceMasterRepository : RepositoryBase<Servicemaster>, IServiceMasterRepository
    {
       public ServiceMasterRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
