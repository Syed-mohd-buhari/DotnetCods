using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class BudgetProjectTrackersRepository : RepositoryBase<Budgetprojecttrackers>, IBudgetProjectTrackersRepository
    {
        public BudgetProjectTrackersRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
