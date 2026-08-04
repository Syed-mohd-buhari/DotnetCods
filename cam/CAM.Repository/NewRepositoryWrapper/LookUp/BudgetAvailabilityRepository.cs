using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class BudgetAvailabilityRepository : RepositoryBaseNew<Budgetavailability>, IBudgetAvailabilityRepository
    {
        public BudgetAvailabilityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}