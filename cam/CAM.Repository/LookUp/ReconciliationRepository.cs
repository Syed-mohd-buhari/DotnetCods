using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public partial class ReconciliationRepository : RepositoryBase<Reconciliations>,IReconciliationRepository
    {
       public ReconciliationRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }
     
    }
}
