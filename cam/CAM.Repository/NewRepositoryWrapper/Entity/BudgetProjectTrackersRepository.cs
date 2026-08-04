using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class BudgetProjectTrackersRepository : RepositoryBaseNew<Budgetprojecttrackers>, IBudgetProjectTrackersRepository
    {
        public BudgetProjectTrackersRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
