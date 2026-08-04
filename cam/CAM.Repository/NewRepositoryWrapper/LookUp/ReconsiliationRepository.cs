using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class ReconsiliationRepository : RepositoryBaseNew<Reconciliations>, IReconciliationRepository
    {
       public ReconsiliationRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
