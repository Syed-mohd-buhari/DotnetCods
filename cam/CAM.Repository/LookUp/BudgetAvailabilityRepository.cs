using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class BudgetAvailabilityRepository : RepositoryBase<Budgetavailability>, IBudgetAvailabilityRepository
    {
        public BudgetAvailabilityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}