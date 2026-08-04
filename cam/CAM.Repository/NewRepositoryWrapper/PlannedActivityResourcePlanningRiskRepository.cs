using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class PlannedActivityResourcePlanningRiskRepository : RepositoryBaseNew<Plannedactivityresourceplanningrisk>, IPlannedActivityResourcePlanningRiskRepository
    {
        public PlannedActivityResourcePlanningRiskRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}